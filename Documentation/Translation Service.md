# Project: LudoTranslation
* Alters strings using reflection and source files
## Flow:
<br>

![GitHub Logo](/Documentation/Images./graph.png)
<br>
First of all we have .lang files which contains lines of the respective translation and it's correspondent property name. The content is split using double equal signs.
<br>
Example: `Game_H1Title==Välkommen till Ludo!`
<br>

![bild](https://user-images.githubusercontent.com/70197523/120020039-7f5cb780-bfe9-11eb-85d3-92bcf1631f6f.png)
<br><br>
Now on the InitializeLanguage() method will firstly make a new instance of the class Dict that contains the properties.
Secondly, a StreamReader will reach each line in the .lang file and find a property with the same name as the left section in that new instance of Dict. If it finds such property it will set that value to it's correspondant left section. Then this process is repeated until the entire file has been read.<br><br>
![bild](https://user-images.githubusercontent.com/70197523/120020686-58eb4c00-bfea-11eb-91c9-5982bced1e3f.png)


As a final result you will get back the newly instanced Dict class and can access all the properties with it's values.<br>
Then just set the chosen content value to any property of Dict.<br><br>

### Why this way?
<br>
We chose to structure it this way since it is very simple to extend the translation afterwards.<br>
If we want to add a completely new string, the only thing we gotta do is to create that property in Dict.cs and then add that property and it's value to the language file.
<br>
If we instead wish to add another language we just need to name the file a 2-character ISO standard country name(SE, US, RU...) and then .lang.<br>
In that file just copy over the properties and set the desired values.<br><br>

**Example load:**
<br><br>
![bild](https://user-images.githubusercontent.com/70197523/120021278-242bc480-bfeb-11eb-96f1-60fadae121d9.png)
<br>
With reflection it becomes very simple.
