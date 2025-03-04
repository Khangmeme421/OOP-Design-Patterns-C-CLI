# GeoQuiz Application

A C# console application demonstrating Object-Oriented Programming (OOP) concepts and Design Patterns through an interactive quiz system.

## Project Overview

This project implements a quiz application that allows users to:
- Create, manage, and play quizzes
- Track scores and completion times
- View high scores
- Get help with system navigation

## Key Features

- Multiple question types (True/False, Multiple Choice, Open-Ended)
- Score tracking and timing system
- High score leaderboard
- Question management system
- Interactive menu navigation

## Object-Oriented Programming Concepts

### 1. Inheritance
- Abstract base class `Menu` with derived menu classes (MainMenu, PlayMenu, etc.)
- Abstract base class `Question` with derived question types:
  - `TrueFalseQuestion`
  - `MultipleChoiceQuestion`
  - `OpenEndedQuestion`

### 2. Encapsulation
- Private fields with public properties in classes like `User` and `Question`
- Protected access modifiers for base class members
- Data hiding through property accessors (get/set)

### 3. Abstraction
- Abstract methods in base classes:
  - `DisplayMenu()` in Menu class
  - `CheckAnswer()`, `DisplayQuestion()`, `GetCorrectAnswer()` in Question class
- Interface-like behavior through abstract classes

### 4. Polymorphism
- Method overriding in derived classes
- Different implementations of abstract methods
- Runtime polymorphism through base class references

## Design Patterns

### 1. Singleton Pattern
- Implemented in `DataManagement` class
- Ensures single instance for managing questions and game state
- Thread-safe implementation using double-check locking

### 2. Factory Pattern
- `MenuFactory`: Creates different types of menus
- `QuestionFactory`: Creates different types of questions
- Encapsulates object creation logic
- Provides flexibility in adding new types

### 3. Back Tracking
- Menu navigation system using recursive approach
- `EndlessMenu` method enables navigation between different menus
- Maintains menu history for "Back" functionality

## Class Structure

### Core Classes
- `Program`: Main entry point and menu loop
- `DataManagement`: Singleton for data persistence
- `User`: Player information and scoring
- `Question`: Base class for all question types

### Menu System
- Abstract `Menu` class
- Specialized menu classes for different functions:
  - `MainMenu`
  - `ManageQuestionsMenu`
  - `PlayMenu`
  - `HelpMenu`
  - And more...

### Question Types
- `TrueFalseQuestion`
- `MultipleChoiceQuestion`
- `OpenEndedQuestion`

## Features

1. **Question Management**
   - Create new questions
   - View all questions
   - Delete questions
   - Edit existing questions

2. **Quiz Play**
   - Answer questions
   - Score tracking
   - Time tracking
   - View correct answers

3. **Score System**
   - Points-based scoring
   - Time tracking
   - High score leaderboard
   - Score persistence using CSV

4. **Help System**
   - Navigation guidance
   - System instructions
   - Menu explanations

## Technical Implementation

- Written in C#
- Console-based interface
- File-based persistence (CSV for high scores)
- Thread-safe singleton implementation
- Recursive menu navigation
- Stopwatch implementation for timing

## Best Practices

- Separation of concerns
- Single Responsibility Principle
- DRY (Don't Repeat Yourself)
- Encapsulated data management
- Protected class members
- Robust error handling
- Input validation

## Usage

1. Run the application
2. Navigate using the main menu
3. Create questions or start playing
4. Follow on-screen instructions
5. View scores and progress

The application provides an intuitive interface for both quiz creation and gameplay, demonstrating practical implementation of OOP concepts and design patterns in a real-world application.
