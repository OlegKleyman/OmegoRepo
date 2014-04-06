Feature: Utorrent
	As a developer I need to be able to
	get information from the utorrent web API

Background:
	Given I added all torrents
	And I have attained an API key

Scenario: Get utorrent key
	When I call the method GetKey
	Then It should result in returning the key to use for this utorrent session

Scenario: Get a torrent
	When I call the method GetTorrentFile with a hash of "D4AD03979D0676F22A0724599FE96FC8BD610877"
	Then It should return a torrent with build number "30303"
	And the torrent should have a hash value of "D4AD03979D0676F22A0724599FE96FC8BD610877"
	And the torrent should have a count of "1" files
	And the file names should be
		| Name                                                         |
		| Some.Show.S01E11.mkv |

Scenario: Update RSS Feeds
	Given Retrieved all RSS feeds
	Then I want to update all RSS feeds

Scenario: Remove torrent
	Given I have attained an API key
	And I have a torrent with the hash of D4AD03979D0676F22A0724599FE96FC8BD610877
	When I call the Remove method on it
	Then the torrent should be removed list