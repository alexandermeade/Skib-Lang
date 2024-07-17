
all: 
	dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true && dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true
