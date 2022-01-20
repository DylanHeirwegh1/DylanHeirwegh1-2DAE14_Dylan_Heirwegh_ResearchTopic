# Flappy bird AI - Neural Network

## Introduction
### Aim
Create an AI that learns to play flappy bird on its own.
### Technique
 The AI will use neural networks, which falls under machine learning.
How does a neural network work
Neural networks act like a brain. They receive data, learn from it and then they try to predict the results.
A neural network has a couple of important components:
- Input layer
- hidden layers
- Output layer

 The hidden layers do the most of the work. They contain multiple layers of neurons that have a specific value, which are connected to each other by channels. Those channels all have a weight which defines how much the output of that neuron matters.
When reaching the output layer, the result of the values combined with the weights will help the AI predict a result. This is called ***Forward Propagation***.

If we have forward propagation we also have ***Backward Propagation***. This is the interesting part of a neural network. When the AI has made a prediction, we will check if it is a good or wrong one. Depending on the result, we will influence the weights. After that, the process starts all over again, until the training process is completed. And that is when the computer is right in most of the cases.

## Implementation
I have downloaded a unity template of the game Flappy bird (https://github.com/dgkanatsios/FlappyBirdStyleGame). This template only provided me the game and the template depends on manual input, so no AI is present in the template. I made some tweaks to the template and I have designed an AI for the game. The neural networks consists of a couple of things:
### nodes
- Three input nodes:
       - The velocity of the player
       - The difference between the height of the player and the height of the tube
       - The direction of the movement of the pipes (The pipes have vertical movement but by a lack of time, I have disabled this input layer)
 - Four hidden nodes
 - one output node 
       - Jump or not
### weights
Al of the previous nodes are connected by weights. These weights are randomly assigned to the birds in the beginning of the game. Every generation, I will take the best two birds and use their weights to make a new generation.
### regeneration
When all of the birds are dead, I spawn a new generation of birds. Like stated before I will use the two best performing birds of the previous generation. Doing that, I am imitating the process of natural selection and hoping to get the best set of weights that way.

### mutations
While making a new population, I am taking a mutation chance into account. That way, we randomize a singe weight (the chance is 5%). The main reason for this is to get some diversity into the population.

## Problems
Somethimes I can see that the AI is clearly avoiding the pipes (I can see the input nodes have effect), but the problem I am encountering is that the AI don't take the input into account as much as needed, or so it would seem. The AI learns very slow and that way, it doesn't get a high score.

## Conclusion
The results aren't what I've hoped for, the birds aren't learning the way I was expecting. They make the same mistakes a lot of the times. Although sometimes I see that they are clearly taking some input factors into account, they don't seem to get a high score.

## Future work
This project is very interesting to me. The fact that the result isn't what I wanted/expected will not stop me to keep on learning about it. This project is something I want to/will finish for sure!

## Template source
https://github.com/dgkanatsios/FlappyBirdStyleGame
