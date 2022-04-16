# Project
A personal blog built with ASP.NET Core MVC with Class Library. Uses Entity Framework Core and deployed with a PostgreSQL database. 

Summary Info

![My App](./app.png)

## WALKTHROUGH
A blog that allows the owner to post articles upon being authenticated via login. Users may leave comments after registering and being authenticated. Tags help filter posts by topic. 

- Blog
- Posts
- Comments
- Tags

## OPEN REQUIREMENTS

MVP:
Blog will have role based access so that allows the owner create posts. Visitors may add comments after registering and logging in. 

OTHER FEATURES:
- DEMO login (will not update database)
- Guest comments with CAPTCHA (does not appear until owner approves it). 
- IP blocking of accounts that abuse the guest comments section.
- Locate comments by guest

## USER INTERFACE
Dashboard shows recent articles with a side-menu to older articles. Search function. Login for writing posts or comments. 

## LOGIC DESIGN
Identity Access, owner creates posts
Role Based Security, authenticated visitors may leave comments


## DATA DESIGN
- Blog Post
- Blog Tag
- Blog Comment
- File (Photos)

-- Users--
- Photo
- Contact Links
- Collection<Posts
- Collection<Comments

--Blog--
- Name
- Description
- Date Created
- Date Updated
- Image
- Creator
-Collection<Posts

--Comment--
- Subject
- Comment
- Date Created
- Date Updated
- Date Moderated
- Date Deleted
- fk_Image
- fk_Post
- fk_creator

--File--
- IFormFile
- FileName
- FileExtension
- FileData
- DateUploaded

--Post--
- Title
- Summary
- Post
- Status
- Date Created
- Date Updated
- Slug
- Photo
- fk_blo
- fk_creator
- Collection<Tags
- Collection<Comments

--Tag--
- Tag
- fk_post
- fk_creator


