# Project
A personal blog built with ASP.NET Core MVC with Class Library. Uses Entity Framework Core and deployed with a PostgreSQL database. 

Summary Info

![My App](./app.png)

## WALKTHROUGH
A blog that allows the owner to post articles upon being authenticated via login. Users may leave comments after registering and being authenticated. Tags help filter posts by topic. 

- Series
- Articles
- Comments
- Tags

## OPEN REQUIREMENTS

MVP:
Owner can create series and write articles under a series.

COMPLETED FEATURES:
- Add role-based access
- add image service
- Add email service for registration and contact me form
- Implement tags 
- Slug service (alternate routes for accessing articles instead of primary keys)
- Visitors may add comments after registering and logging in. 
- Locate comments by guest
- Add role-based views

USER INTERFACE
- user can click tags to find other articles with that tag
- Implement Series as a nav bar in shared _layout with searcher

- Article cards for landing page
	- featured article with photo, series, title, and summary (links to article)
	- 2nd-5th most recent articles below featured article at half size

FUTURE UPDATES:
- Hover series to display carousel of articles within that series

- Series index
	- featured article with photo series, title, and summary (links to article)
	- 2nd-5th most recent articles below featured article at half size

- Article page
	- Series
	- Title
	- Summary
	- AuthorImage Author
	- Article Image
	- Article Body
	- Show/Hide comments

- DEMO login (will not update database)
- Guest comments with CAPTCHA (does not appear until owner approves it). 
- IP blocking of accounts that abuse the guest comments section.

## USER INTERFACE
Dashboard shows series and recent articles. Side-menu to older articles. Search function. Login for writing articles or comments. 

## LOGIC DESIGN
Identity Access, owner creates posts
Role Based Security, authenticated visitors may leave comments


## DATA DESIGN
- Blog Series
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
- fk_Image
- fk_Creator
-Collection<Articles

--Comment--
- Subject
- Comment
- Date Created
- Date Updated
- Date Deleted
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
- fk_series
- fk_creator
- Collection<Tags
- Collection<Comments

--Tag--
- Tag
- fk_post


