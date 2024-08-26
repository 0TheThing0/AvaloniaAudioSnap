# AudioSnap — audio metadata at your fingertips

AudioSnap is a project that strives to make audio metadata fetching easier by utilizing audio fingerprinting algorithms and making use of extensive audio metadata storages. The primary goal of this project was to get familiar with audio recognition algorithms and techniques.  

AudioSnap project is composed of 3 key components:
- [AudioSnap microservice] — microservice to recognize fingertips and fetch metadata from services
- AudioSnap client — the current repository
- [Chromaprint library] — included in both the microservice and the client

The inspiration for the project comes from [this article][chromaprint article], that goes deep into audio recognition algorithms that formed the basis of the following web APIs our project relies on:
- [AcoustID] — recognize audio by its fingerprint
- [MusicBrainz] — fetch audio metadata by its ID, relies on AcoustID
- [CoverArtArchive] — fetch link to audio cover art, relied on MusicBrainz/AcoustID

## Disclaimer

> Although **there ARE solutions to the problem** provided by AcoustID and MusicBrainz themselves (e.g. [MusicBrainz Picard]), our project's goal was to — again — get familiar with the audio fingerprinting algorithms and build something that can be used as a convenient mean of providing metadata to audio files, analagous to solutions provided by MusicBrainz and AcoustID.  
Our goal was not to build a counterpart OR competitor to these solutions by any means, as our solution strongly relies on their services and their audio fingerprinting algorithms. Neither was our goal to monetise the application, even though it is allowed on a paid basis.  
Same goes to the rewritten [open-source chromaprint library][original chromaprint library], which is basically the implementation of the audio fingerprinting algorithms described in [this article][chromaprint article]. The goal was to get familiar with the algorithms, and we believe that the best way to learn is to practise.
>
## Manual build

In order to build the project you'll have to add a reference to [our Chromaprint library][Chromaprint library] package. Once the library project has been built, you must provide the compiled library to the appropriate location. The location where the current project expects to find the compiled ```.dll``` file is provided in ```AvaloniaAudioSnap.csproj```, and is currently equal to: ```./Packages/Chromaprint.dll```.

# Conclusion
Considering this repository, it can be said that, as a result, a client has been an interesting project, in which I tried to create client-server application using AvaloniaUI. Ofcourse, this app is far away from being perfect: code is "dirty", the structure is far from ideal, some libraries I used are not multiplatform. But it was interesting and educational to try something so big, especially with someone else (thanks [SignificantNose])


[AudioSnap microservice]: <https://github.com/0TheThing0/AvaloniaAudioSnap>
[Chromaprint library]: <https://github.com/0TheThing0/Chromaprint_lib>
[Original chromaprint library]: <https://github.com/acoustid/chromaprint/tree/master>
[Chromaprint article]: <https://oxygene.sk/2011/01/how-does-chromaprint-work/>

[AcoustID]: <https://acoustid.org/>

[MusicBrainz]: <https://musicbrainz.org/>

[CoverArtArchive]: <https://coverartarchive.org/>

[MusicBrainz Picard]: <https://picard.musicbrainz.org/>

[SignificantNose]: <https://github.com/SignificantNose>
