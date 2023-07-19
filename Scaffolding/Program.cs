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

        var modelOption = new Option<string>(
            name: "--model",
            description: "Nombre del modelo/entidad",
            getDefaultValue: () => "ModelExample"
            );

        var folderlOption = new Option<string>(
            name: "--folder",
            description: "Nombre de la carpeta/namespace",
            getDefaultValue: () => "Shared"
            );

        var makeCommand = new Command("make")
        {
            modelOption,
            folderlOption
        };

        var rootCommand = new RootCommand
        {
            makeCommand
        };

        rootCommand.Description = "Scaffolding para crear archivos en SARA.Server";

        makeCommand.SetHandler(async (modelOptionValue, folderOptionValue) =>
        {
            string entityName = $"{modelOptionValue}Entity";
            string entityFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{ModelsFolder}/{folderOptionValue}");
            CreateFileModel(entityName, entityFilePath, folderOptionValue);

            string repositoryName = $"{modelOptionValue}Repository";
            string interfaceName = $"I{repositoryName}";

            string IRepositoryFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{RepositoriesFolder}/{folderOptionValue}/Interfaces");
            CreateFileInterfaceRepository(interfaceName, entityName, IRepositoryFilePath, folderOptionValue);

            string repositoryFilePath = Path.Combine($"{RootFolder}{ProjectFolder}{RepositoriesFolder}/{folderOptionValue}");
            CreateFileRepository(repositoryName, interfaceName, entityName, repositoryFilePath, folderOptionValue);

        }, modelOption, folderlOption);

        // Parse the incoming args and invoke the handler
        await rootCommand.InvokeAsync(args);
    }

    static void CreateFileModel(string entityName, string filePath, string folderNamespace)
    {
        string templateFilePath = "Templates/EntityTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);

        string folderNamespaceFormated = folderNamespace.Replace('/', '.');

        string fileContent = templateContent
        .Replace("{NAMESPACE}", folderNamespaceFormated)
        .Replace("{ENTITY_NAME}", entityName);

        Path.Combine(filePath, $"{entityName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{entityName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {entityName}.cs creado con éxito. ✅");
    }

    static void CreateFileRepository(string repositoryName, string IRepositoryName, string entityName, string filePath, string folderNamespace)
    {
        string templateFilePath = "Templates/RepositoryTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);

        string folderNamespaceFormated = folderNamespace.Replace('/', '.');

        string fileContent = templateContent
        .Replace("{NAMESPACE}", folderNamespaceFormated)
        .Replace("{REPOSITORY_NAME}", repositoryName)
        .Replace("{ENTITY_NAME}", entityName)
        .Replace("{INTERFACE_NAME}", IRepositoryName);

        Path.Combine(filePath, $"{repositoryName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{repositoryName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {repositoryName}.cs creado con éxito. ✅");
    }

    static void CreateFileInterfaceRepository(string IRepositoryName, string entityName, string filePath, string folderNamespace)
    {
        string templateFilePath = "Templates/IRepositoryTemplate.txt";
        string templateContent = File.ReadAllText(templateFilePath);

        string folderNamespaceFormated = folderNamespace.Replace('/', '.');

        string fileContent = templateContent
        .Replace("{NAMESPACE}", folderNamespaceFormated)
        .Replace("{ENTITY_NAME}", entityName)
        .Replace("{INTERFACE_NAME}", IRepositoryName);

        Path.Combine(filePath, $"{IRepositoryName}.cs");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string filePathCombine = Path.Combine($"{filePath}/{IRepositoryName}.cs");

        File.WriteAllText(filePathCombine, fileContent);
        Console.WriteLine($"Archivo {IRepositoryName}.cs creado con éxito. ✅");
    }
}