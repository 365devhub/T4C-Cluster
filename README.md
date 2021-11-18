# T4C-Cluster 
![](https://github.com/365devhub/T4C-Cluster/actions/workflows/BuildMaster.yml/badge.svg)

This project aims to create a T4c Server emulator.
The Emulator Is developped in C# Therefore, it is not based on any existing leaked code.


## General informations

The emulator will be made of multiple server called nodes. Together, they form an interconnected cluster.
This project uses the **Actor** patern,actor persistence and the custer uses sharding and actor node balancing.
Persistence means that in case of an actor faillure or even a complete node failure, that actor that no longuer online will be automatically recreated with the sme data they had before going down. Sharing and node balancing means that actors are spread evenly across the cluster using load balancing and rebalancing.

There will be no direct memory access between actors, so no need for mutex or semaphore.


<details open><summary><strong>Network Node :</strong></summary>
<p>
Basically just a proxy / load balancer. It distributes the client request to the server nodes.
</p>
</details>

<details open><summary><strong>Player Node :</strong></summary>
  <p>
    <br/>
    PlayerActor :
    <ul>
      <li>One PlayerActor by player</li>
      <li>Handles all of the request made by players.</li>
      <li>It also handle event sent between players and the game's world.</li>
      <li>Actors are aware of all other actor in it environement and have basic informations. But if and Action on a other actor is required, is will be sent to that actor.</li>
    </ul>
  </p>
  <p>
    <br/>
    ChatterActor :
    <ul>
      <li>Handles the chatter channels</li>
      <li>Only one for the whole cluster.</li>
    </ul>
  </p>
</details>





