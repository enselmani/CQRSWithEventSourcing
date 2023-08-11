# CQRS With Event Sourcing
A simple, unambitious Microservices Architecture, CQRS Pattern with Event Sourcing, and DDD implementation in .NET

# Getting Started:

## Prerequisites:
* .NET 7 </br>
* Docker </br>

## Setting up the environment:
* Create a Docker network: </br>
  <code>docker network create --attachable -d bridge mydockernetwork</code> </br>
* Run the required containers: </br>
  <code>docker-compose up -d</code> </br>
* Create the Kafka topic: </br>
  <code>docker exec [<em>kafka container unique Id</em>] kafka-topics.sh</code> </br>
  <code>--bootstrap-server localhost:9092 --create --replication-factor 1</code> </br>
  <code>--partitions 1 --topic SocialMediaPostEvents</code> </br>

## Might be helpful to use:
* A client software for SQL Server &nbsp;&nbsp;&nbsp; (Ex. <em>SSMS</em>) </br>
* A client software for MongoDB &nbsp;&nbsp;&nbsp;&nbsp; (Ex. <em>Studio 3T</em>) </br>
* For testing REST endpoints &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Ex. <em>Postman</em>) </br>

## Final steps:
* Set **Multiple startup projects** to run both services.
* Import the <em>Postman_collection</em> file into Postman.
* Start testing :)

## Your feedback is welcomed! :)

