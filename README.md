# Project
A personal blog built with ASP.NET Core MVC with Class Library. Uses Entity Framework Core and deployed with a PostgreSQL database. 

Summary Info

![My App](./app.png)

## WALKTHROUGH
A blog that allows the owner to post articles upon being authenticated via login. Users may leave comments after registering and being authenticated. 


## OPEN REQUIREMENTS

MVP:
Blog will have role based access so that allows the owner create posts. Visitors may add comments after registering and logging in. 

OTHER FEATURES:
- Guest comments with CAPTCHA (does not appear until owner approves it). 
- IP blocking of accounts that abuse the guest comments section.
- Locate comments by guest

## USER INTERFACE
Dashboard shows recent articles with a side-menu to older articles. Search function. Login for writing posts or comments. 


## LOGIC DESIGN
Identity Access, owner
Role Based Security


## DATA DESIGN
- Blog Post
- Blog Tag
- Blog Comment

-- Blogger --
- Collection of Posts