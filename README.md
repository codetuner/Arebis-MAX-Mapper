# Max (Arebis-MAX-Mapper)

A generator for both contract objects (Data Transfer Objects) and back and forth mapping based on an Xml contract definition. Powerfull, extensible,...

## Introduction

In its blogpost [Why You Shouldn’t Expose Your Entities Through Your Services](https://github.com/davybrion/companysite-dotnet/blob/master/content/blog/2010-05-why-you-shouldnt-expose-your-entities-through-your-services.md), Davy Brion explains why exposing entities through your service is not a good idea, instead you should expose DTO's that are shaped according to the needs of the service operation.

Max is meant to provide an easy way to define DTO types based on your entity/domain types, with support for complex transformations including over collections (auto-detecting added/updated/removed members), hierarchy flattening and unflattening, graph flattening and unflattening, calculated properties, etc. without the need to write complex mapping code. In fact, (almost) all code is generated for you!

Among some great features of Max, i'd like to mention:

* The generation of both DTO types and mapping code (in 2 directions!) based on a single mapping definition (in XML, with schema-based intellisense) !

* How easily you can add functionality to the DTO's or to the mapper code: simply Add Project Item and choose the additional generations you want ! Or create yourself additional reusable generation templates.
 
* The best practices implemented in the generation templates, for instance providing the ability to implement IEntityWithKey without exposing this interface through your service contract !
 
* Its support for Entity Framework, but also the fact it isn't coupled to it, making it an ideal mapping suite for NHibernate or other ORMs as well !
 
* How nicely all generation templates and generation code is hidden - though still visible - as behind files of a single root generation template.

## Getting Started

To get you introduced to Max I've created a few video's that give you a quick introduction in just a few minutes. Be aware that the video's show only a very limited set of features of the Max Mapping Generator. For a more complete sample, see the Getting Started section of the documentation.

* Visit the Videos page (temporarily not available anymore since mogration from Codeplex)

## Manual

The manual (currently in PDF format) is available in the downloads section:

* Download the Manual (2MB PDF) (temporarily not available anymore since mogration from Codeplex)
* Or visit the Manual page (temporarily not available anymore since mogration from Codeplex)
