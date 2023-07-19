# Scaffolding

Esta herramienta fue realizada para automentar la productividad en un proyecto de la empresa donde actualmente trabajo.

Scaffolding nos permite generar de manera automatica clases, entidades, repositorios e interfaces, con una sola linea de codigo, con esto reducimos la cantidad de codigo inicial que se debe escribir.

La herramienta se puede seguir ampliando pero hasta ahora hace lo que necesitamos.


## Compilar:

Deberia tener alojado el proyecto en la carpeta donde Visual Studio almacena los respositorios por defecto "source/repos".

```sh
cd C:\Users\your-name-pc\source\repos\Scaffolding\Scaffolding
dotnet build
```

## Ejecutar en consola:


```sh
cd C:\Users\your-name-pc\source\repos\Scaffolding\Scaffolding\bin\debug\net7.0

.\Scaffolding --help

.\Scaffolding make --model EncabezadoMuestra --folder "Mobile/CalidadLeche"
```
