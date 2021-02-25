namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using a3innuva.TAA.Migration.SDK.Extensions;
    using a3innuva.TAA.Migration.SDK.Implementations;
    using a3innuva.TAA.Migration.SDK.Interfaces;
    using a3innuva.TAA.Migration.SDK.Serialization;
    using a3innuva.Tutorial.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class App
    {
        private readonly IZipUtil zipUtil;
        private readonly IConfigurationRoot config;
        private readonly IDataGenerator dataGenerator;

        public App(IZipUtil zipUtil, IConfigurationRoot config, IDataGenerator dataGenerator)
        {
            this.zipUtil = zipUtil ?? throw new ArgumentNullException(nameof(zipUtil));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.dataGenerator = dataGenerator ?? throw new ArgumentNullException(nameof(dataGenerator));
        }

        public async Task Run()
        {
            var entities = this.dataGenerator.GenerateRandomDate(500);

            List<IMigrationEntity> list = new List<IMigrationEntity>();

            int index = 0;

            IValidation<Account> validation = new AccountValidation(); 

            foreach (var item in entities.Where(x => x.Group.StartsWith("4")))
            {
                //Mapeo
                Account account = new Account()
                {
                    Id = item.Id,
                    Code = item.Group + item.Code,
                    Description = item.Name,
                    Name =  item.Name,
                    Line = index,
                    VatNumber = item.VatNumber,
                    PostalCode = item.PostalCode,
                };

                //Validacion
                var result = validation.Validate(account);

                if(result.ToList().TrueForAll(x => x.IsValid))
                    list.Add(account);
                else
                    Console.WriteLine($"{item.Group}{item.Code} Error");
                    
                index++;
            }

            //Construir el SET
            MigrationSet set = new MigrationSet
            {
                Entities = list.ToArray(),
                Info = new MigrationInfo()
                {
                    VatNumber = "S8558911G", //El nif debe ser el de la empresa de a3innuva
                    FileName = string.Empty, // Opcional, informar en caso de que haga referencia a un fichero fisico
                    Origin = MigrationOrigin.Extern, // Origen externo
                    Type = MigrationType.ChartOfAccount, //Tipo de fichero.
                    Version = "2.0",
                    Year = 0 //Necesario en caso de asientos y facturas
                    //La longitud del plan contable no se informa pero debe coincidir con la de la empresa en a3innuva
                }
            };

            //Podemos validar los datos de la cabecera
            var isInfoValid  = set.Info.IsValid();

            Console.WriteLine($"Set info is valid {isInfoValid}");

            //Podemos validar un set completo
            var isSetValid = set.IsValid();

            Console.WriteLine($"Set info and entities are valid {isSetValid}");

            //var outputPath = this.config.GetValue<string>("PathConfigurationSettings:OutputPath");
            var outputPath = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = this.config.GetValue<string>("PathConfigurationSettings:OutputFile");
            var path = $"{outputPath}{fileName}";

            //Serializar
            await using (StreamWriter outputFile = new StreamWriter(path, false, Encoding.UTF8))
            {
                using JsonTextWriter writer = this.CreateWriter(outputFile);
                var jsonSettings = JsonSerializationSettingsUtils.GetSettings();
                string json = JsonConvert.SerializeObject(set, jsonSettings);
                await writer.WriteRawAsync(json).ConfigureAwait(false);
            }


            //Crear el zip
            this.zipUtil.CreateZipFile(outputPath, path);
        }

        private JsonTextWriter CreateWriter(StreamWriter outputFile)
        {
            JsonTextWriter writer = new JsonTextWriter(outputFile)
            {
                AutoCompleteOnClose = true,
            };

            return writer;
        }
    }
}
