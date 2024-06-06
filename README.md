# ByteBag

A ByteBag egy web alkalmazás ami egy tech témával foglalkozó 
fórum és használt tech eszközöket árusító marketplace.
Kiegészítésként [Hulvej Szabolcs](https://github.com/szabixd) által fejlesztett Adminsztrációs Asztali alkalmazásban kezelhetjük a felhasználók fiókjait és posztjait.


## Tartalomjegyzék
- [Funkciók](#funkciók)
- [Használat](#használat)
- [Tesz fiókok](#teszt-fiókok)
- [Kompromisszumok](#kompromisszumok)
- [Dokumentáció](#dokumentáció)
- [Branch-ek](#branch)
- [Fejlesztők](#fejlesztők)
- [Külső library-k](#külső-library-k)

  

## Funkciók
- Forum
- Koment lehetőség
- Marketplace
- Chat a marketplace-hez
- WPF admin alkalmazás
- verzió kezelés
- Login with github funció
- Elfelejtett jelszó vissza állítása E-mail kóddal


  
## Használat
Futtasd a szervert localhost-on!
Admin jogosultség esetén a WPF alkamazás is igénybe vehető amit az oldalon keresztül könnyen le lehet tölteni.

## Teszt fiókok:
  - Felhaszunálónév: teszt, Jelszó: teszt123 (Sima felhasználó)
  - Felhaszunálónév: admin, Jelszó: admin123 (Admin felhasználó)

## Kompromisszumok
A weboldal a localhost-os futtatás miatt sajnos egy két funkció hiányosan vagy nem működik. Ezek a következők:
- Login with Gitub
- E-mail értesítő
- Elfelejtett jelszó
- SSL tanusítvány

  Ezek hiányosságok mint adatvédelmi szempontok miatt van. Nem szeretnénk privát kulcsokat és belépési adatokat publikálni. Viszont a [weboldalon](https://bytebag.hu) ezek a funkciók gond nélkül üzemelnek és ki is próbálhatók.



## Dokumentáció
[Dokumentáció](./ByteBagDokumentacio.docx)



## Branch
  - [WPF alkalmazás branch-e](https://github.com/csuszy/ByteBag/tree/Wpf)
    
  - [WEB alkalmazás branch-e](https://github.com/csuszy/ByteBag/tree/Web)



## Fejlesztők
- [Császi Bence](https://github.com/csuszy)
- [Hulvej Szabolcs](https://github.com/szabixd)
- [Bencze Andrés Krisztián](https://github.com/Jegenye0)


## Külső library-k
[Network Helper](https://github.com/vellt/Network_Helper_Library)
