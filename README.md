# Project
A personal blog built with ASP.NET Core MVC with Class Library. Uses Entity Framework Core and deployed with a PostgreSQL database. 

Summary Info

![My App](./app.png)

## WALKTHROUGH
A blog that allows the owner to post articles upon being authenticated via login. Users may leave comments after registering and being authenticated. Tags help filter posts by topic. 

- Topics
- Articles
- Comments
- Tags

## OPEN REQUIREMENTS

MVP:
Role based access; only owner creates articles. Visitors may add comments after registering and logging in. 

OTHER FEATURES:
- DEMO login (will not update database)
- Guest comments with CAPTCHA (does not appear until owner approves it). 
- IP blocking of accounts that abuse the guest comments section.
- Locate comments by guest

## USER INTERFACE
Dashboard shows topics and recent articles. Side-menu to older articles. Search function. Login for writing articles or comments. 

## LOGIC DESIGN
Identity Access, owner creates posts
Role Based Security, authenticated visitors may leave comments


## DATA DESIGN
- Blog Topic
- Blog Article
- Blog Tag
- Blog Comment
- Photos

-- Users--
- Photo
- Contact Links
- Collection<Articles
- Collection<Comments

--Blog--
- Name
- Description
- Date Created
- Date Updated
- Image
- Creator
-Collection<Articles

--Comment--
- Subject
- Comment
- Date Created
- Date Updated
- Date Moderated
- Date Deleted
- fk_Image
- fk_Article
- fk_creator

--File--
- IFormFile (Image)
- PhotoName
- PhotoExtension
- PhotoData
- DateUploaded

--Article--
- Title
- Summary
- Body
- Status
- Date Created
- Date Updated
- Slug
- Photo
- fk_topic
- fk_creator
- Collection<Tags
- Collection<Comments

--Tag--
- Tag
- fk_post
- fk_creator


