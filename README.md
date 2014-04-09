# OpenLawOffice

Status: Development

## What?
OpenLawOffice is a web (HTTP) based client-server system for management of larm firm resource including matters, billing, tasking and documents.

## Why?

### Why the project?
The only task more difficult than managing a law office without software dedicated to that purpose is swallowing the pricetag hanging on the existing systems.  Worse yet, some public sector lawyers are overworked and underpaid.  OpenLawOffice will step into this field to provide the software to assist these lawyers and support staff to help prevent errors, manage tasks and allow the directors of the departments to put more of the budget in their employee's pockets.  OpenLawOffice is open source software that is freely available to anyone, period.  

### Why your involvement?
It's the new Pro-Bono!  Legal services are expensive, no question.  One large factor in the cost to market for these services is technology.  Almost all aspects of a law department can now be virtualized using technology.  We simply mean to increase the availability of reduced total cost of ownership technology for law office management so these firms and departments can deliver the same or higher quality services to their clients at a lower total cost.


## How?

### Will you stay afloat offering free software?
Contributions and value added services.  For instance, who better to perform your site analysis, setup your servers and bring OpenLawOffice active in your environment than the people writing the program?  As we freely give our time and intellectual property to the development of OpenLawOffice, we simply ask that your installation and support issues get directed to us so we can keep the lights on for the project.

## Can We (I) Help?
Absolutely!  There is much to be done.  If you would like to assist, simply contact Lucas Nodine [github](https://github.com/lucasnodine) or [linkedin](http://www.linkedin.com/profile/view?id=15557533) or if you prefer non-social media sites, please visit the [Nodine Legal](http://www.nodinelegal.com/) website for multiple direct contact methods.  Please only contact us if you are serious about contributing to the project.

## Installation
### Step 1 - Prerequisites
The below are currently considered the prerequisites and this software is only tested within the Visual Studio (R) 2010 IDE with Postgres 9.2 all on windows 7.

*.NET Framework v4
*MVC2
*Postgresql

### Step 2 - Database
Create a database within your postgresql server.  It doesn't matter what you call it, you can change the name in the web.config disucssed later.

### Step 3 - Webapp
* Modify the "fileStorageSettings" to point to existing locations.  This will be where your documents are stored, so make sure you have ample space.
* Modify your connection string
* Modify the DbProviderFactories as needed.  I have it commented out as I have Npgsql installed on my system (See 2.2 http://npgsql.projects.pgfoundry.org/docs/manual/UserManual.html for details on installing).

### Step 4 - Let me know
If you find problems with installation, file an issue and let me know.
