# Project: LudoDataAccess; EmailClient
Generates a HTML Email body and sends it to clients using SMTP-protocol.
## Flow:
<br>
First of all we instantiate a new SMTP client using Google Server, since our host email is using Gmail.
<br>
<br>

![bild](https://user-images.githubusercontent.com/70197523/120022388-a36dc800-bfec-11eb-9681-1ffeb35c5d96.png)

<br>
We then wish to get a list of all the account objects connected to the recipients. This is because we want the e-mail to be formatted in the receivers preferred language using TranslationEngine.<br><br>

![bild](https://user-images.githubusercontent.com/70197523/120022562-dc0da180-bfec-11eb-89fb-f682671b22ba.png)

<br>
We are also sending the emails separately using a for-loop. This is of two reasons:
<ol>
  <li>We don't want to leak any data to the recipients. Like other receivers.</li>
  <li>We want the e-mail to be translated individually based on the recipients preferred language</li>
  </ol>
Also, notice the email body is set using the method GenerateBody(). This method basically imports our HTML-body and then alters it for the user using simple string.replace.
<br><br>

![bild](https://user-images.githubusercontent.com/70197523/120022760-25f68780-bfed-11eb-9bf4-c82388b3df2e.png)
