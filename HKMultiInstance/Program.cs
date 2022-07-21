using SuccincT.Functional;

const string dataDir = "hollow_knight_Data";
const string steamAppId = "steam_appid.txt";

string dir = Environment.CurrentDirectory;
string? exe = Directory.GetFiles(dir, "hollow_knight.*").FirstOrDefault();
if (exe == null || !Directory.Exists(dataDir))
{
    Console.WriteLine("Could not find HK files here");
    return;
}

var (action, (instance, rest)) = args;
switch (action)
{
    case "create":
        CreateInstance(instance);
        break;
    case "delete":
        DeleteInstance(instance);
        break;
    default:
        Console.WriteLine("Invalid action: " + action);
        break;
}

string GetInstanceExeName(string name) => exe!.Replace("hollow_knight", name);

string GetInstanceDataName(string name) => $"{name}_Data";

bool InstanceExists(string name) => Directory.Exists(GetInstanceDataName(name)) || File.Exists(GetInstanceExeName(name));

void CreateInstance(string name)
{
    if (InstanceExists(name))
    {
        Console.WriteLine($"Failed to create instance: {name}\nInstance already exists");
        return;
    }
    Directory.CreateSymbolicLink(GetInstanceDataName(name), dataDir);
    File.Copy(exe!, GetInstanceExeName(name));
    if (!File.Exists(steamAppId))
    {
        using StreamWriter sw = File.CreateText(steamAppId);
        sw.Write("367520");
    }
    Console.WriteLine($"Successfully created instance {name}");
}

void DeleteInstance(string name)
{
    if (!InstanceExists(name))
    {
        Console.WriteLine($"Failed to delete instance: {name}\nInstance does not exist");
        return;
    }
    Directory.Delete(GetInstanceDataName(name));
    File.Delete(GetInstanceExeName(name));
    Console.WriteLine($"Successfully deleted instance {name}");
}