-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3306
-- Létrehozás ideje: 2024. Máj 03. 07:46
-- Kiszolgáló verziója: 8.2.0
-- PHP verzió: 8.2.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `s43625_szakdoga`
--
CREATE DATABASE IF NOT EXISTS `s43625_szakdoga` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `s43625_szakdoga`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `chattomarket`
--

DROP TABLE IF EXISTS `chattomarket`;
CREATE TABLE IF NOT EXISTS `chattomarket` (
  `chatID` int NOT NULL AUTO_INCREMENT,
  `userID` int NOT NULL,
  `marketID` int NOT NULL,
  PRIMARY KEY (`chatID`),
  KEY `userID` (`userID`,`marketID`),
  KEY `marketID` (`marketID`)
) ENGINE=InnoDB AUTO_INCREMENT=165 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `chattomarket`
--

INSERT INTO `chattomarket` (`chatID`, `userID`, `marketID`) VALUES
(164, 57724668, 278);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `comment`
--

DROP TABLE IF EXISTS `comment`;
CREATE TABLE IF NOT EXISTS `comment` (
  `commentID` int NOT NULL AUTO_INCREMENT,
  `posztID` int NOT NULL,
  `userID` int NOT NULL,
  `commentText` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `commentDATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`commentID`),
  KEY `posztID` (`posztID`,`userID`),
  KEY `userID` (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=319 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `comment`
--

INSERT INTO `comment` (`commentID`, `posztID`, `userID`, `commentText`, `commentDATE`) VALUES
(318, 142, 57724668, 'https://www.digitalocean.com/community/tutorials/initial-server-setup-with-ubuntu\n\nItt biztos találsz megoldást majd', '2024-05-03 09:43:27');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `desktopversion`
--

DROP TABLE IF EXISTS `desktopversion`;
CREATE TABLE IF NOT EXISTS `desktopversion` (
  `id` int NOT NULL AUTO_INCREMENT,
  `version` text NOT NULL,
  `downloadUrl` text NOT NULL,
  `releaseNotes` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- A tábla adatainak kiíratása `desktopversion`
--

INSERT INTO `desktopversion` (`id`, `version`, `downloadUrl`, `releaseNotes`) VALUES
(3, '1.1.4', 'ByteBag-1.1.4.msi', 'asdasd'),
(4, '1.1.4', 'ByteBag-1.1.4.msi', 'Teszt'),
(5, '1.2.2', 'ByteBag-1.2.2.msi', 'verziószámok'),
(6, '1.2.3', 'ByteBag-1.2.3.msi', 'verziószám fix'),
(7, '1.2.3.0', 'ByteBag-1.2.3.0.msi', '1.2.3.0 formátum'),
(8, '1.2.4.0', 'ByteBag-1.2.4.0.msi', 'Verziószám teszt, update Window új item.'),
(9, '1.2.5.0', 'ByteBag-1.2.5.0.msi', 'frissítés teszt');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `forum`
--

DROP TABLE IF EXISTS `forum`;
CREATE TABLE IF NOT EXISTS `forum` (
  `posztID` int NOT NULL AUTO_INCREMENT,
  `userID` int NOT NULL,
  `title` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `post` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `hashtag` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `postDATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`posztID`),
  KEY `userID` (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=143 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `forum`
--

INSERT INTO `forum` (`posztID`, `userID`, `title`, `post`, `hashtag`, `postDATE`) VALUES
(142, 57724667, 'Szerver beállítása', 'Egy kis segítség kellene otthoni ubuntu szerver beállításhoz. Valaki ért hozzá?', NULL, '2024-05-03 09:34:39');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `market`
--

DROP TABLE IF EXISTS `market`;
CREATE TABLE IF NOT EXISTS `market` (
  `marketID` int NOT NULL AUTO_INCREMENT,
  `userID` int NOT NULL,
  `title` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `marketpost` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `price` bigint NOT NULL,
  `marketDATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`marketID`),
  KEY `userID` (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=279 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `market`
--

INSERT INTO `market` (`marketID`, `userID`, `title`, `marketpost`, `price`, `marketDATE`) VALUES
(278, 57724667, 'Fractal Design Pop Air Tower Fekete', 'Fractal Design Pop Air. \r\nKülső megjelenés: Tower, \r\nTípus: PC, \r\nTermék színe: Fekete. \r\nTámogatott tápegység formatények: ATX. \r\nBeszerelt első ventilátorok: 2x 120 mm, \r\nKompatibilis méretű első ventilátorok: 120,140 mm, \r\nBeszerelt hátsó ventilátorok: 1x 120 mm. \r\nTámogatott merevlemez-meghajtó méretek: 2.5,3.5,5.25, \r\nSSD formatényező: 2.5. \r\nSzélesség: 215 mm,\r\nMélység: 473,5 mm, \r\nMagasság: 454 mm', 36400, '2024-05-03 09:41:49');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `marketmessage`
--

DROP TABLE IF EXISTS `marketmessage`;
CREATE TABLE IF NOT EXISTS `marketmessage` (
  `userID` int NOT NULL,
  `chatID` int NOT NULL,
  `message` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `sendDATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  KEY `userID` (`userID`,`chatID`),
  KEY `chatID` (`chatID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `marketmessage`
--

INSERT INTO `marketmessage` (`userID`, `chatID`, `message`, `sendDATE`) VALUES
(57724668, 164, 'Szia, ez még elérhető?', '2024-05-03 09:44:35'),
(57724667, 164, 'Igen persze, Debrecenben akár meg is lehet nézni', '2024-05-03 09:45:05'),
(57724668, 164, 'Ez jól hangzik!', '2024-05-03 09:45:33');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `marketpicture`
--

DROP TABLE IF EXISTS `marketpicture`;
CREATE TABLE IF NOT EXISTS `marketpicture` (
  `imgID` int NOT NULL AUTO_INCREMENT,
  `marketID` int NOT NULL,
  `path` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`imgID`),
  KEY `marketID` (`marketID`)
) ENGINE=InnoDB AUTO_INCREMENT=61531 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `marketpicture`
--

INSERT INTO `marketpicture` (`imgID`, `marketID`, `path`) VALUES
(61524, 278, '1714722109582.webp'),
(61525, 278, '1714722109582.webp'),
(61526, 278, '1714722109582.webp'),
(61527, 278, '1714722109583.webp'),
(61528, 278, '1714722109583.webp'),
(61529, 278, '1714722109583.webp'),
(61530, 278, '1714722109583.webp');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `userID` int NOT NULL AUTO_INCREMENT,
  `githubID` int DEFAULT NULL,
  `username` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `email` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `password` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `profilPic` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `admin` tinyint(1) NOT NULL DEFAULT '0',
  `registerDATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=57724669 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`userID`, `githubID`, `username`, `email`, `password`, `profilPic`, `admin`, `registerDATE`) VALUES
(57724667, NULL, 'teszt', 'teszt@teszt.hu', '$2b$10$6DPPbtwF9mtkzDLYdONIFeGMFEnhhEuEWA2CcjnU5BkdUqIWVTnfS', '', 0, '2024-05-03 09:29:06'),
(57724668, NULL, 'admin', 'admin@admin.hu', '$2b$10$8CB0dmxN.Ea./Nq.3fwtQuJ2aYbej61ZZlxsGNixwZ1G7WB6CfD6m', '', 1, '2024-05-03 09:31:35');

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `chattomarket`
--
ALTER TABLE `chattomarket`
  ADD CONSTRAINT `chattomarket_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `chattomarket_ibfk_2` FOREIGN KEY (`marketID`) REFERENCES `market` (`marketID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `comment_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `comment_ibfk_2` FOREIGN KEY (`posztID`) REFERENCES `forum` (`posztID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `forum`
--
ALTER TABLE `forum`
  ADD CONSTRAINT `forum_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `market`
--
ALTER TABLE `market`
  ADD CONSTRAINT `market_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `marketmessage`
--
ALTER TABLE `marketmessage`
  ADD CONSTRAINT `marketmessage_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `marketmessage_ibfk_2` FOREIGN KEY (`chatID`) REFERENCES `chattomarket` (`chatID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `marketpicture`
--
ALTER TABLE `marketpicture`
  ADD CONSTRAINT `marketpicture_ibfk_1` FOREIGN KEY (`marketID`) REFERENCES `market` (`marketID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
