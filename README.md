# Parser Generator
## COMP SCI 4TB3 Final project

**Comparison of Parsing Techniques.**
Implement the P0 parser using a parser generator and verify that it runs as efficiently or better than the existing P0 parser or other parsing techniques.

### Prerequisite Software
Install the following software:
[dotnet core SDK](https://dotnet.microsoft.com/download)

### Building
Run the following commands from the folder containing this readme document:

```
$ cd parsergenerator
$ dotnet restore
```

To do a run of the application without a test file, run the following (This uses the sample factorial program as input to the parser):
```
$ dotnet run
```


If you have a P0 program in a file you want to test, run the following:
```
$ dotnet run <filename.ext>
```

To run the benchmark for the application, run the following instead where n is the number of iterations to run the test:
```
$ dotnet run bench <n>
```


### Tests
Run the following commands from the folder containing this readme document.
```
$ cd parsergenerator.Tests
$ dotnet restore
$ dotnet test
```
The dotnet core CLI will inform you on how many tests ran successfully, and how many failed if any.