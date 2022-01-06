# Flappy bird AI - Neural Network

## Introduction
### Aim
#### Create an AI that learns to play flappy bird on its own.
### Technique
#### The AI will use neural networks, which falls under machine learning.
### How does a neural network work
#### Neural networks act like a brain. They receive data, learn from that data and then they try to predict the results from that data.
#### A neural network has a couple of important components:
- Input layer
- hidden layers
- Output layer
#### The hidden layers do the most of the work. They contain multiple layers of neurons that have a specific value, which are connected to each other by channels. Those channels all have a weight which defines how much the output of that neuron matters.
#### When reaching the output layer, the result of the values combined with the weights will help the AI predict a result. This is called ***Forward Propagation***.
#### If we have forward propagation we also have ***Backward Propagation***. This is the interesting part of a neural network. When the AI has made a prediction, we will check if it is a good or wrong one. Depending on the result, we will influence the weights. After that, the process starts all over again, until the training process is completed. And that is when the computer is right in most of the cases.
