using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace Scaffolding;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        string? RootFolder;
        string? ProjectFolder;
        string? RepositoriesFolder;
        string? ModelsFolder;

        string? enviroment = configuration["Enviroment"];

        if (enviroment == "Production")
        {
            RootFolder = configuration.GetSection("FoldersPathProduction:Root").Value;
            ProjectFolder = configuration.GetSection("FoldersPathProduction:Project").Value;
            RepositoriesFolder = configuration.GetSection("FoldersPathProduction:Repositories").Value;
            ModelsFolder = configuration.GetSection("FoldersPathProduction:Models").Value;
        }
        else
        {
            RootFolder = configuration.GetSection("FoldersPathDevelopment:Root").Value;
            ProjectFolder = configuration.GetSection("FoldersPathDevelopment:Project").Value;
            RepositoriesFolder = configuration.GetSection("FoldersPathDevelopment:Repositories").Value;
            ModelsFolder = configuration.GetSection("FoldersPathDevelopment:Models").Value;
        }

        var makeArgument = new Argument<string>(
            name: "make",
            description: "Crear archivos");

        var modelOption = new Option<string>(
            name: "--model",
            description: "Nombre del modelo",
            getDefaultValue: () => "MyModel"
            );

        var rootCommand = new RootCommand
        {
            modelOption
        };

        rootCommand.Description = "App para crear archivos del proyecto SARA";

        rootCommand.SetHandler(async (modelOptionValue) =>
        {
            string entityName = $"{modelOptionValue}Entity";
            string entityFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{ModelsFolder}");
            CreateFileModel(entityName, entityFilePath);

            string repositoryName = $"{modelOptionValue}Repository";
            string interfaceName = $"I{repositoryName}";

            string IRepositoryFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{RepositoriesFolder}/Interfaces");
            CreateFileInterfaceRepository(interfaceName, entityName, IRepositoryFilePath);

            string repositoryFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{RepositoriesFolder}");
            CreateFileRepository(repositoryName, interfaceName, entityName, repositoryFilePath);

        }, modelOption);

        // Parse the incoming args and invoke the handler
        await rootCommand.InvokeAsync(args);
    }

    static void CreateFileModel(string fileName, string filePath)
    {
        string templateFilePath = "Templates/EntityTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);

        string fileContent = templateContent
        .Replace("{NAMESPACE}", "Example")
        .Replace("{ENTITY_NAME}", fileName);

        Path.Combine(filePath, $"{fileName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{fileName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {fileName}.cs creado con éxito.");
    }

    static void CreateFileRepository(string repositoryName, string IRepositoryName, string entityName, string filePath)
    {
        string templateFilePath = "Templates/RepositoryTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);


        string fileContent = templateContent
        .Replace("{NAMESPACE}", "Example")
        .Replace("{REPOSITORY_NAME}", repositoryName)
        .Replace("{ENTITY_NAME}", entityName)
        .Replace("{INTERFACE_NAME}", IRepositoryName);

        Path.Combine(filePath, $"{repositoryName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{repositoryName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {repositoryName}.cs creado con éxito.");
    }

    static void CreateFileInterfaceRepository(string IRepositoryName, string entityName, string filePath)
    {
        string templateFilePath = "Templates/IRepositoryTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);

        string fileContent = templateContent
        .Replace("{NAMESPACE}", "Example")
        .Replace("{ENTITY_NAME}", entityName)
        .Replace("{INTERFACE_NAME}", IRepositoryName);

        Path.Combine(filePath, $"{IRepositoryName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{IRepositoryName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {IRepositoryName}.cs creado con éxito.");
    }
}