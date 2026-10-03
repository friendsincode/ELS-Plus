#!/usr/bin/env python3
"""Compile existing .NET Framework sources without Windows deployment hooks.

Requires Python 3.9+, a .NET SDK, NuGet access, and the referenced repository DLLs.
Outputs/logs stay in a temporary directory; this does not package a resource.
"""
from pathlib import Path
import subprocess
import sys
import tempfile
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
NS = {"m": "http://schemas.microsoft.com/developer/msbuild/2003"}
PROJECTS = {
    "SharpConfig": ROOT / "libs/SharpConfig-master/SharpConfig/SharpConfig.csproj",
    "Client": ROOT / "src/ELS.csproj",
    "Server": ROOT / "server/ELS-Server/ELS-Server.csproj",
}


def create_project(name, original, output):
    source = ET.parse(original)
    project = ET.Element("Project", Sdk="Microsoft.NET.Sdk")
    properties = ET.SubElement(project, "PropertyGroup")
    settings = {
        "TargetFramework": "net452",
        "LangVersion": "7.3",
        "GenerateAssemblyInfo": "false",
        "EnableDefaultCompileItems": "false",
        "EnableDefaultEmbeddedResourceItems": "false",
        "EnableDefaultNoneItems": "false",
        "AssemblyName": name,
    }
    for key, value in settings.items():
        ET.SubElement(properties, key).text = value
    items = ET.SubElement(project, "ItemGroup")
    ET.SubElement(items, "PackageReference",
                  Include="Microsoft.NETFramework.ReferenceAssemblies.net452", Version="1.0.3")
    if name == "Client":
        packages = ET.parse(original.parent / "packages.config")
        client = next(p for p in packages.getroot() if p.attrib["id"] == "CitizenFX.Core.Client")
        ET.SubElement(items, "PackageReference", Include=client.attrib["id"], Version=client.attrib["version"])
    for entry in source.findall("m:ItemGroup/m:Compile", NS):
        path = original.parent / entry.attrib["Include"].replace("\\", "/")
        ET.SubElement(items, "Compile", Include=str(path))
    for entry in source.findall("m:ItemGroup/m:Reference", NS):
        name_only = entry.attrib["Include"].split(",")[0]
        if name_only == "CitizenFX.Core.Client":
            continue
        reference = ET.SubElement(items, "Reference", Include=entry.attrib["Include"])
        for child in entry:
            tag = child.tag.rsplit("}", 1)[-1]
            if tag != "HintPath":
                ET.SubElement(reference, tag).text = child.text
        hint = entry.find("m:HintPath", NS)
        if hint is not None:
            path = original.parent / hint.text.replace("\\", "/")
            ET.SubElement(reference, "HintPath").text = str(path.resolve())
    if name != "SharpConfig":
        ET.SubElement(items, "ProjectReference", Include=str(output / "SharpConfig/SharpConfig.csproj"))
    directory = output / name
    directory.mkdir()
    ET.indent(project)
    ET.ElementTree(project).write(directory / f"{name}.csproj", encoding="unicode")


def main():
    output = Path(tempfile.mkdtemp(prefix="els-managed-check-"))
    print("Build logs and outputs:", output, flush=True)
    for name, original in PROJECTS.items():
        create_project(name, original, output)
    failed = False
    for name in ("Client", "Server"):
        result = subprocess.run(
            ["dotnet", "build", str(output / name / f"{name}.csproj"), "--nologo", "-v:q"],
            capture_output=True, text=True,
        )
        log = result.stdout + result.stderr
        (output / f"{name}.log").write_text(log)
        failed |= result.returncode != 0
        print(f"{name}: {'FAILED' if result.returncode else 'passed'}", flush=True)
        if result.returncode:
            print(log, flush=True)
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
