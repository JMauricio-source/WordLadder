# WordLadder
Challenge to implement a Word Ladder game solver

## Scope
The project wordladder is an implementation of a solver for the word ladder problem.
The Word ladder challenge consists of finding a sequence of words that differ from the siblings by just one letter in the same position.
The solver should return one of shortest lists or conclud that is not possible to resolve given the parameters (start word, end word, word List).

The purpose of this project was to propose and efficient and correct implementation for the solver using modern framework (net Core 3.1)
and programming concepts using appropriate design patterns to promote a good architeture design for testability and scalability. 

## Technology

The project was written in C# using Microsoft .Net Core 3.1 framework and is a Console application. 

## Code structure/Architecture

The WordLadder project consiste of three projects plus a Test project:
### 1. WordLadder - 
    Console App that hosts a BackgroundWorker service that runs the actual processor.
### 2. WordLadder.Models - 
    Contain classes for type definition only used in the other two projects.
### 3. WordLadder.Services - 
    Interface declarations and service implementation for injected services.
    Here we define the services:
        - CommandLinePayloadLoader - Loads the arguments passed by the user and creates a JobPayloadCommand that will define the search parameters.
        - WordLadderProcessor - The class that constains the Word Ladder Algoritm.
        - WordListRepository - Abstracts the data aquisition layer. Readsthe word list from a txt file and make the lines availables to the app.
        - 

## The Algorithm

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. 
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.

## Final notes

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. 
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
