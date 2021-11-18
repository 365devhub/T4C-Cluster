# T4C-Cluster

This project aims to create a T4c Server emulator.
The Emulator Is developped in C# Therefore, it is not based on any existing leaked code.



## General informations

The emulator will be made of multiple server called nodes.

                       |   ________
                       |  |        | 
                       |  | WORKER | <---+
                       |  |________|     |
 _________             |   ________      |
|         | ---------> |  |        |     |
| NETWORK |            |  | WORKER | <---+
|________ | <--------- |  |________|     |
                       |   ________      |
                       |  |        |     |
                       |  | WORKER | <---+    
                       |  |________| 
                       |
