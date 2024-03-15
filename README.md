# pushmonolith
Simple cloud deployment tool for a spring boot application.

### Package the Pushmonolith.Cli
```bash
cd .\pushmonolith\src\Pushmonolith.Cli\
dotnet pack --output ../../nupkg
```

### Install Pushmonolith.Cli
```bash

# install as a global tool
dotnet tool install --global pushmonolith.cli

# uninstall 
dotnet tool uninstall -g pushmonolith.cli
```

### Use Pushmonolith.Cli
```bash

# Authentication
pushmonolith login

pushmonolith init .

pushmonolith deploy .

# shows all applications
pushmonolith ls
```
