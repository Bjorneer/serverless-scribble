# Serverless Scribble Hosting With Azure Functions

[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

# [DEMO]([demo-url]) 
(Note: There is proably going to be a cold start of the azure function so the first request to a Azure function may take some time a few seconds)



<details open="open">
  <summary>Content</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#how-to-play">How To Play</a></li>
        <li><a href="#architecture">Architecture</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#setup">Setup</a>
    </li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



## About The Project

This project is a real-time serverless multiplayer drawing game where one player receives a word and has to paint it while the other players in the lobby try to guess the word. It is hosted on Azure with the backend part as a Azure Function App written in .Net Core and the frontend hosted as a React.js Static Website in a Storage Account to save on cost. It also uses Azures SignalR service to provide a real-time application for the users.  

### How To Play

### Architecture

![alt text][architecture]

The backend part of the system is built solely with Azure functions and their durable extension for stateful lobby and game orchestration. The flow is as follows:
1. A client creates a game by supplying a username which starts a new LobbyOrchestration.
2. Any numbner of players can now join the game by supplying a username and the game code for that lobby.
  2,5. On join/create the player also connect to a SignalR group for that lobby which is now responible for sending events such as other players joining.
3. The lobby owner decides to start the game which signals SignalR and turns on the GameOrchestrator (represents a single round)
4. The GameOrchestrator now picks a painter and a random word taken from a generated storage table and sends signals to all players.
5. The painter can now see the word and paint on their canvas. Sending ajax requests to the Draw endpoint which then through the SignalR service sends that information to the other players, currently with a 2 second delay so that the receiver can process and not have to paint everything at once. 
6. Players are at the same time as the player is drawing able to send guesses to a api endpoint which validates it and either sends a message to other players that he was correct or if he was wrong it sends that guess to the other players to display in the chat. 
7. The GameOrchestrator restarts itself which creates a new round.


### Built With

* [React](https://reactjs.org/) - Frontend
* [.Net Core](https://docs.microsoft.com/en-us/dotnet/fundamentals/) - Backend framework
* [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) - Backend API and game loop

## Setup

1. Clone the repo
   ```sh
   git clone https://github.com/Bjorneer/serverless-scribble.git
   ```

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

Filip Björnåsen - filip@mosquito.se

[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=for-the-badge
[license-url]: https://github.com/Bjorneer/serverless-scribble/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/filip-bj%C3%B6rn%C3%A5sen-b07a7a1b3/
[architecture]: Assets/azure-scribble-architecture.PNG
[demo-url]: https://scribblestorage.z16.web.core.windows.net/
