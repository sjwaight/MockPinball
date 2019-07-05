# Mock Pinball Simulator

This project is designed as a simulator for a pinball machine wired up for IoT.

You can build this project on any platform using [Visual Studio Code](https://code.visualstudio.com/download) and [.Net Core](https://dotnet.microsoft.com/download).

In order to run it you will need to create an [IoT Hub](https://azure.microsoft.com/free/iot/) in Microsoft Azure (there is a free tier which will suffice for running this demo).

Configure your device in IoT Hub and then copy and paste the SAS connection string into the Program.cs file before your compile.

You can run the Pinball simulator as one of two players - AI or human. AI is the default.

```
# Run as AI
dotnet run .

# Run as AI
dotnet run . on

# Run as Human
dotnet run . off
```