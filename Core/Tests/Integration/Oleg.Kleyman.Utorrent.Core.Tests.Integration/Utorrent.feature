Feature: Utorrent
	As a developer I need to be able to
	get information from the utorrent web API

Scenario: Get utorrent key
	When I call the method GetKey
	Then It should result in returning the key to use for this utorrent session

Scenario: Get a torrent
	Given I have attained an API key
	When I call the method GetTorrentFile with a hash of "FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC"
	Then It should return a torrent with build number "27498"
	And the torrent should have a hash value of "FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC"
	And the torrent should have a count of "2" files
	And the file names should be
		| Name               |
		| daa-alvh-1080p.mkv |
		| daa-alvh-1080p.nfo |

Scenario: Update RSS Feeds
	Given I have attained an API key
	And Retrieved all RSS feeds
	Then I want to update all RSS feeds