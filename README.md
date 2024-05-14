# CoffeeMachine

Web API which emulate imaginary coffee machine.

## endopoint

- `/brew-coffee`
- Return a message that your coffee is ready
- Return a iced coffee message if the temperature is 30 C or hotter in Auckland(NZ).
- Return out of coffee error every five times
- Return I'm a teapot error when you request it on April fool.

## environment

- .Net Core 8.0
- Visual Studio 2022

## launch

1. Clone this project
1. Open Solution file on Visual Studio 2022
1. Open `appsettings.json` and set a value to `WeatherApiKey`
1. Choose `Coffee Machine` as a start up project
1. Choose `http` and start debug
