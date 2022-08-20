This is the source code for Promptu (https://www.promptulauncher.com/)

## Background

Promptu is the largest passion project from my early days as a software engineer, building something I used everyday when I was primarily a Windows user. Development was driven by things I wanted personally and feature requests from family/friends/users.

I learned a ton while building it, and the fact I was using it everyday (and seeing it directly make my life easier) kept my motivation high through the many obstacles along the way.

However, my ability to contribute slowed down when I went to university in 2010, and eventually stopped completely by the time I joined Google full time in 2014.

Around that same time, I primarily became a Linux user.

I did start exploring what it would take to port to Linux (using Mono, hooking into X11 for hotkeys & how to make p/invoke work with , UI abstraction so it could feel native on each platform, etc) but those efforts stalled out because my day job took up all my engineering energy.

The efforts to abstract the UI are present in this codebase already, since I had made the switch from WinForms to WPF with eventually supporting other platforms in mind.

## Here be dragons

If I was starting over, there's a lot I would change. True for all projects, but especially ones from early in your career.

Here's some notable ones which contributors should consider fixing:

> I didn't really write unit tests - mostly hand tested everything
* This worked *okay* when Promptu was my main project, but once I got to university and splitting time on other projects, it quickly became impossible to make changes and know it was going to be safe. yeahhh...really kicking myself on that one.
> The [plugin model](https://www.promptulauncher.com/docs/writing-external-functions.php) allows you to write arbitrary code and run it as part of your commands to transform or source strings. Great for flexibility! Amazing!
* but oh wait, it's running as part of the promptu process without any kind of sandboxing
    * ...yeahhh that's not good
* but wait there's more...there's also a [command list syncing mechanism](https://www.promptulauncher.com/docs/lists.php#list-sharing) which zips up the commands list and DLLs that include that arbitrary code you wrote, and propogates that to anyone who is sharing that same list on your local network share
    * attack vector anyone? ...holy shit wtf was I thinking

## Positive highlights

* TrieList/TrieDictionary - fast character based searching and partial matching. The idea to use a tree of nodes of character dictionaries to keep the number of search steps small came to me the morning after studying the tree example in the K&R ANSI C book. I later found out it was a pre-existing data structure called a trie.


## Tools used

* Visual Studio
* Microsoft Expression Blend (for the WPF UI)
* Microsoft Expression Design (for generating vector icons for the WPF UI & rendering raster counterparts where needed) - looks to be defunct now

