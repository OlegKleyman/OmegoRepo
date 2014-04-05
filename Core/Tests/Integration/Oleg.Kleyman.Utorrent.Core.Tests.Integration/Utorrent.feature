Feature: Utorrent
	As a developer I need to be able to
	get information from the utorrent web API

Scenario: Get utorrent key
	When I call the method GetKey
	Then It should result in returning the key to use for this utorrent session

Scenario: Get a torrent
	Given I have attained an API key
	When I call the method GetTorrentFile with a hash of "25A7640F5E8BDC73EBC08E28D8CD4B044CCEF182"
	Then It should return a torrent with build number "30303"
	And the torrent should have a hash value of "25A7640F5E8BDC73EBC08E28D8CD4B044CCEF182"
	And the torrent should have a count of "1" files
	And the file names should be
		| Name                                                         |
		| Минута славы - Мечты сбываются! - Второй полуфинал_bySat.mpg |

Scenario: Update RSS Feeds
	Given I have attained an API key
	And Retrieved all RSS feeds
	Then I want to update all RSS feeds