# T4C-Cluster ![](https://github.com/365devhub/T4C-Cluster/actions/workflows/BuildMaster.yml/badge.svg)

This project aims to create a T4c Server emulator.
The Emulator Is developped in C# Therefore, it is not based on any existing leaked code.



## General informations

The emulator will be made of multiple server called nodes.

**Network Node :**
Basically juste a proxy or load balancer. It distributes the client request to the server nodes



**Player Node :**
Handles all of the request made by players.
It also handle event sent between players and the game's world.



