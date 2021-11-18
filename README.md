# T4C-Cluster 
![](https://github.com/365devhub/T4C-Cluster/actions/workflows/BuildMaster.yml/badge.svg)

This project aims to create a T4c Server emulator.
The Emulator Is developped in C# Therefore, it is not based on any existing leaked code.


## General informations

The emulator will be made of multiple server called nodes.

<details><summary><strong>Network Node :</strong></summary>
<p>
Basically just a proxy / load balancer. It distributes the client request to the server nodes
</p>
</details>



<details><summary><strong>Player Node :</strong></summary>
  <p>
    <br/>
    PlayerActor :
    <ul>
      <li>One PlayerActor by player</li>
      <li>Handles all of the request made by players.</li>
      <li>It also handle event sent between players and the game's world.</li>
    </ul>
  </p>
  <p>
    <br/>
    ChatterActor :
    <ul>
      <li>Only one for the whole cluster</li>
      <li>Handles the chatter channels</li>
    </ul>
  </p>
</details>





