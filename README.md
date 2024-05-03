# ByteBag

A ByteBag egy web alkalmazás ami egy tech témával foglalkozó 
forum és használt tech eszközöket árusító marketplace ahol
a felhasználók eladhatják az ilyen eszközöket. 
Emellé még tartoyik egy WPF alkalmazás amiben az admin funkciókat használahtjuk.



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
- Marketplace
- Chat a marketplace-hez
- WPF admin alkalmazás
- verzió kezelés
- Login with github funció



  
## Használat
Látogass el a https://bytebag.hu weboldalra és már használható is.
Admin jogosultség esetén a WPF alkamazás is igénybe vehető amit az oldalon keresztül könnyen le lehet tölteni.

## Teszt fiókok:
  - Felhaszunálónév: teszt, Jelszó: teszt123 (Sima felhaszunáló)
  - Felhaszunálónév: admin, Jelszó: admin123 (Admin felhaszunáló)

## Kompromisszumok
A weboldal a localhost-os futtatás miatt sajnos egy két funkció hiányosan vagy nem működik. Ezeka következők:
- Login with Gitub
- E-mail értesítő
- Elfelejtett jelszó
- ssl tanusítvény

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
