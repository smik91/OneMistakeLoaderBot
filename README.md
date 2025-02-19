# Telegram Video Bot (.NET 8.0)

A Telegram bot written in .NET 8.0 that can upload videos up to 50 MB (due telegram restrictions)
## Features
- Handles video uploads (up to 50 MB)
- Basic command handling in a Telegram chat

## Requirements
- .NET 8.0
- A valid Telegram Bot Token

## How to Run
1. Clone the repository:  
   ```
   git clone https://github.com/smik91/OneMistakeLoaderBot
   ```
2. Navigate to the project folder:  
   ```
   cd OneMistakeLoaderBot
   ```
3. Restore and build the project:  
   ```
   dotnet restore  
   dotnet build  
   ```
4. Set your Telegram Bot Token (e.g., in appsettings.json or via an environment variable).  
5. Run the bot:  
   ```
   dotnet run  
   ```
   
## Usage
- Open a chat with your bot in Telegram.
- Use /start to begin.
- Send a link on video (up to 50 MB). The bot will process or echo it back.

Feel free to open an issue or submit a pull request for any improvements.
