Czym jest test jednostkowy
==========================

To kod, który wykonywany jest w zdefiniowanych, kontrolowanych warunkach
w celu weryfikacji czy testowany fragment działa w określony, z góry
zdefiniowany sposób.

Test składa się z 3 etapów: dostarczenia danych wejściowych, wykonania
testowanych instrukcji i sprawdzenia czy ich rezultaty są zgodne z
oczekiwaniami.

Ważne! Test nie sprawdza czy kod działa tak jak powinien tylko czy
działa tak jak zakłada to jego autor.

Rodzaje testów
==============

-   testy jednostkowe --- testujemy pojedynczą, jednostkową część kodu:
    zazwyczaj klasę lub metodę;

-   testy integracyjne --- testujemy kilka komponentów systemu
    jednocześnie;

-   testy regresyjne --- po wprowadzeniu zmiany uruchamiane są wszystkie
    testy w danej domenie biznesowej celem sprawdzenia czy zmiana nie
    spowodowała błędu w innej części systemu;

-   testy akceptacyjne --- testy mające na celu odpowiedzieć na pytanie
    czy aplikacja spełnia wymagania biznesowe.

Co powinniśmy testować
======================

-   logikę biznesową

-   wyrzucanie wyjątków (świadomie dodane instrukcje throw)

-   sprawdzamy czy kod robi to co powinien i czy nie robi tego czego nie
    powinien

    -   np. jeśli testowana metoda ma przyznać rabat danemu typowi
        użytkownika to sprawdzamy czy nie przyznaje go innemu typowi

-   testów powinno być tyle, żebyśmy się dobrze czuli z releasowaniem
    kodu na produkcję

Czego nie powinniśmy testować
=============================

-   nie testujemy tego co nie ma sensu

-   prostych getterów/seterów

-   metod prywatnych (choć testujemy je poprzez metody publiczne)

-   wygenerowanego kodu

-   nie testujemy szczegółów implementacyjnych tylko rezultaty („co" a
    nie „jak)

Czym jest TDD
=============

Elementem charakterystycznym TDD jest pisanie testu przed napisaniem
docelowego kodu.

Główne cele:

-   Zachowanie wysokiej jakości designu w swoich klasach.

    -   Pisanie testu przed implementacją kodu wymusza przemyślenie
        designu. Na początku rozpoznajemy jakie są interakcje danej
        klasy z innymi obiektami, jakich interfejsów potrzebujemy.

-   Uniknięcie złej interpretacji wymagań biznesowych.

    -   Testy piszemy w kontekście dokumentacji. Osoba pisząca testy
        musi rozumieć intencje jej autorów, wykryć przypadki brzegowe i
        je również przetestować.

-   Zachowanie prostoty w kodzie.

    -   W TDD nie piszemy testów i nie implementujemy kodu do rzeczy,
        których nie potrzebujemy teraz, a które być może ktoś będzie w
        przyszłości potrzebować.

Cykl pisania testów, czyli Red-Green-Refactor
=============================================

1.  **Red** -- piszemy test

    1.  Możemy mieć puste klasy/metody i dzięki temu korzystać z
        IntelliSense.

    2.  Test po uruchomieniu nie powinien przejść.

    3.  Pisząc testy pokrywamy również przypadki brzegowe i wyjątkowe.

    4.  Kod testu powinien być jak najprostszy -- nie chcemy bugów w
        testach.

    5.  Powinien mieć jeden powód do nie przechodzenia -- powinien
        testować jedną rzecz.

2.  **Green** -- implementacja logiki biznesowej

    1.  Po implementacji uruchamiamy testy. Powinny się powieść.

3.  **Refactor** -- poprawiamy jakoś napisanego kodu, bez zmiany jego
    funkcjonalności.

    1.  Testy nadal powinny przechodzić.

Zalety
======

-   Zrozumienie wymagań, wychwycenie braków w specyfikacji

-   Testy jako dokumentacja zawsze aktualna w czasie (pod warunkiem, że
    są regularnie uruchamiane)

-   Wymuszenie dobrego designu kodu, szybka identyfikacja potencjalnych
    problemów z zależnościami

-   Bezpieczniejsze modyfikacje kodu

-   Natychmiastowy i automatyczny feedback na temat potencjalnych błędów
    w kodzie

-   Testy regresyjne -- czy po zmianach nie zepsuliśmy przy okazji
    jakiejś innej funkcjonalności

-   Dużo mniej debugowania w poszukiwaniu błędów

-   Mogą zastąpić komentarze -- test modelujący konkretna informację
    (jak coś się zmieni, to ktoś zmodyfikuje test, brak problemu
    nieaktualnych komentarzy)

Wady
====

-   Czas i wysiłek na trening TDD

-   Testy musza być zarządzane i poprawiane w tym samym czasie co reszta
    kodu

-   Początkowa percepcja dłuższego developmentu

Nazewnictwo
===========

-   Nazwa testu powinna opisywać co testujemy i jakiego rezultatu
    oczekujemy

-   Nie przejmujemy się długimi nazwami testów

*NazwaTestowanejMetody_WhenTestowanyScenariusz_ShouldOczekiwanyRezultat*

Cechy testów
============

-   FIRST

    -   Fast -- szybkie, tak by programista otrzymywał szybką informację
        zwrotną

    -   Independent -- niezależne od innych, odizolowane. Test nie
        powinien uruchamiać kolejnego testu

    -   Repeatable -- powtarzalne (niezależnie ile razy uruchomione
        zwrócą te same rezultaty), powinny po sobie sprzątać (jeśli
        zmieniają coś w środowisku), bez założeń co do stanu
        początkowego (stan możemy przygotować na początku/tu przed
        testem)

    -   Self-checking -- powinny stwierdzać czy przeszły czy też nie
        (bez konieczności analizy logów itp.)

    -   Timely -- pisane razem z kodem produkcyjnym

-   Kod testu powinien być prosty i czytelny

-   Test powinien albo przechodzić albo nie. Nie może być częściowo
    poprawny. Logika działa albo nie

-   Powinien mieć co najmniej jedną asercję. Ktoś może napisać test bez
    asercji, aby sprawdzić czy jego kod nie wyrzuci wyjątku. Framework
    do testowania powinien mieć oddzielną metodę w klasie Assert do
    sprawdzania czy wyjątek został lub nie został wyrzucony przez zadany
    kod.

-   Powinien testować jedno zachowanie. Nie umieszczamy wielu asercji
    testujących różne zachowania w jednym teście jednostkowym. Możemy go
    podzielić na kilka testów.

-   Niezależność od innych elementów wewnętrznych/zewnętrznych -- to co
    potrzebujemy możemy mockować itp.

Złe praktyki kodowania w kontekście testowalności kodu
======================================================

-   Korzystanie ze statycznych klas, metod, pól, singletonów
   
    -   testy mogą wpływać wzajemnie na wynik, mniejsza elastyczność, trudniej napisać test
   
    -   brak izolacji

-   Zależność od stanu globlanego - typu od czegoś w bazie danych, od jakiegoś zdarzenia w systemie, od danych otrzymanych z innego systemu
		
-   Tworzenie innych klas w konstruktorze i metodach, bez możliwości wstrzyknięcia zależności

    -   znaki ostrzegawcze:
        -   operator new	
		-   wywołania statycznych metod
		-   cokolwiek ponad przypisywanie wartości do pól klasy
		-   niepełna inicjalizacja - tj. żeby obiekt był w pełni funkcjonalny musimy coś jeszcze zrobić (np. wywołać init)
		-   instrukcje warunkowe
		-   tworzenie konstruktora specjalnie na potrzeby testów

-   Klasy/metody, które co prawda przyjmują dependencje jako parametr, ale nie można jej zamockować a jest trudna w utworzeniu

-   Metody, które robią "za dużo"

    -   wiele rozgałęzień w ramach metody
   
    -   im więcej odpowiedzialności tym więcej kombinacji do przetestowania, testy robią się dłuższe, mniej czytelne, więcej do mockowania

-   Metody, których zachowanie zależy (a nie można przekazac tego jako argument, nie jest też to generowane przez jakąś dependencję, którą możemy sterować w teście) od: czasu (DateTime.Now, upływ czasu), wartości losowe

-   Kod, który modyfikuje jakiś stan globalny - typu zapisuje coś w bazie danych

-   Kod ze "skutkami ubocznymi" - wysyła coś do innego systemu, wysyła maile


Dobre i złe praktyki w pisaniu testów
=====================================

-   Nie powinniśmy łamać granic enkapsulacji tylko i wyłącznie na
    potrzeby testów. Jeśli musimy coś takiego zrobić to albo mamy coś
    nie tak z naszą architekturą albo testowaną rzecz możemy
    zweryfikować w inny sposób. Np. Zamiast sprawdzać czy status klienta
    został zmieniony możemy sprawdzać czy system zachowuje się jakby ten
    status został zmieniony. Np. Jeśli klient został zablokowany to nie
    powinien móc złożyć zlecenia.

-   Im mniej logiku typu pętle, instrukcje warunkowe w kodzie testu tym
    lepiej (ryzyko popełnienia błędu, trudniejszy kod w zrozumieniu)

    -   Jeśli potrzebujemy instrukcji warunkowej to zamiast tego
        powinniśmy napisać 2 osobne testy

-   Nie testujemy tylko ścieżki optymistycznej, sprawdzamy również
    przypadki brzegowe, wyjątki, nulle

-   Nieaktualne testy powinny być poprawione lub usunięte

-   Testowanie prywatnych składowych: Nie powinno się testować
    prywatnych składowych klasy, powinniśmy testować jedynie publiczne
    API klas. Nie interesuje nas ich wewnętrzna implementacja, wobec
    czego nie powinniśmy testować jej prywatnych składowych.

-   Testy jednostkowe nie powinny mieć żadnej konfiguracji.

-   Testy nie powinny zawierać odwołań do konsoli systemowej. Niektórzy
    używają Console.Writeline w celu sprawdzenia czegoś ręcznie.

-   Łapanie wszystkich wyjątków: Łapanie wszystkich wyjątków i objęcie w
    try-catch może spowodować niewyłapanie błędu w logice testowanego
    kodu. Do asercji związanych z oczekiwaniem wyjątku służą oddzielne
    metody klasy Assert.

-   Jeśli spodziewamy się innego wyjątku niż typu Exception, powinniśmy
    sprecyzować typ oczekiwanego wyjątku.

-   Oczekiwanie na wyjątek w niewłaściwym miejscu: Powinniśmy
    sprecyzować, która część kodu wyrzuci nam wyjątek. Przykładem złej
    praktyki jest stosowanie atrybutu \[ExpectedException\] (NUnit).
    Założenie takiego atrybutu skutkuje, że oczekujemy na wyjątek w
    całym teście. Powinniśmy zastosować metodę Throws klasy Assert,
    która odnosi się do konkretnego kodu, który ma rzucić wyjątek.

**Podział assembly**: Każde assembly powinno zawierać osobne typy testów
i być nazwane wg konwencji: Tests.Unit, Tests.Integration,
Tests.Acceptance, itd. Jest kilka powodów dla których powinniśmy
wprowadzić taki podział:

-   Powinniśmy w szybki sposób móc stwierdzić czy porzebujemy
    odpowiedniej konfiguracji aby uruchomić test oraz czy potrzebny jest
    dostęp do zewnętrznych zasobów.

-   Łatwość stwierdzania czy test czerwony wynika z braku konfiguracji,
    braku dostępu do zależności, błędu logiki w kodzie produkcyjnym, z
    błędu w kodzie testowym lub nieaktualnym kodzie testowym.

-   Ponadto czerwony test jednostkowy ma priorytet dużo większy niż test
    czerwony integracyjny.

Struktura testu
===============

Arrange--Act--Assert

**Arrange**: wszystkie dane wejściowe i *preconditions,*

**Act**: działanie na metodzie/funkcji/klasie testowanej,

**Assert**: upewnienie się, że zwrócone wartości są zgodne z
oczekiwanymi.

Nomenklatura
============

*Dummy* jest najprostszą z atrap, nie robi absolutnie nic! Jego zadaniem
jest tylko i wyłącznie spełnienie założeń sygnatury.

string lastName = It.IsAny\<string\>(); // Dummy

*Stub* to nieco bardziej zaawansowany *dummy*. Dodatkowo jednak, *stub*
potrafi zwracać zdefiniowane przez nas wartości, o ile o nie poprosimy.
*Stub* też nie wyrzuci błędu, jeśli nie zdefiniowaliśmy danego stanu
(np. metody void są puste, a niezdefiniowane wartości wyjścia zwracają
wartości domyślne).

var customer = Mock.Of\<ICustomer\>(c =\> c.GetAge() == 21); // Stub

*Fake* jest z kolei wariancją *stuba* i ma na celu symulowanie bardziej
złożonych interakcji. Jest to z reguły własnoręcznie napisana klasa,
która posiada minimalną funkcjonalność aby spełnić założenia interakcji.

*Mock* potrafi weryfikować zachowanie obiektu testowanego. Jego celem
jest sprawdzenie czy dana składowa została wykonana.

*Spy* to *mock* z dodatkową funkcjonalnoscią. O ile *mock* rejestrował
czy dana składowa została wywołana, to *spy* sprawdza dodatkowo ilość
wywołań.

NUnit
=====

Podstawowe Atrybuty
-------------------

\[SetUp\] i \[TearDown\], \[TestFixtureSetUp\] i
\[TestFixtureTearDown\], \[Test\]

Assert.Throws
-------------

Do testowania wyrzucenia wyjątku.

\[TestCase\]
------------

zestaw wartości dla wszystkich parametrów metody

\[TestCase(4, 2, 2.0f)\]

\[TestCase(-4, 2, -2.0f)\]

\[TestCase(1, 3, 0.333333343f)\]

public void Divide_ReturnsProperValue(int dividend, int divisor, float
expectedQuotient)

\[Values\]
----------

zestaw wartości dla parametru metody

\[Test\]

public void Divide_DividendIsZero_ReturnsQuotientEqualToZero(

\[Values(-2, -1, 1, 2)\] double divisor)

\[Range\]
---------

zakres od-do z zadanym krokiem dla parametru metody

\[Test\]

public void Divide_DividendIsZero_ReturnsQuotientEqualToZero(

\[Range(from: 1, to: 5, step: 1)\] int divisor)

\[Random\]
----------

zakres od-do oraz ilość wartości do wygenerowania dla parametru metody

public void
Divide_DividendAndDivisorAreRandomPositiveNumbers_ReturnsPositiveQuotient(

\[Random(min: 1, max: 100, count: 10)\] double dividend,

\[Random(min: 1, max: 100, count: 10)\] double divisor)

Testowanie klas generycznych
----------------------------

\[TestFixture(typeof(ArrayList))\]

\[TestFixture(typeof(List\<int\>))\]

\[TestFixture(typeof(Collection\<int\>))\]

public class ListsTests\<T\> where T : IList, new()

{

\[Test\]

public void CountTest()

{

var list = new T { 2, 3 };

Assert.That(list, Has.Count.EqualTo(2));

}

}

Moq
===

Nie możemy mockować statycznych klas i metod. Przy tworzeniu obiektu
mocka wywołany zostanie konstruktor i logika związana z inicjalizacją
obiektu.

Mocki w stylu funkcyjnym

Różnice:

-   Składnia dla tworzenia mocka to Mock.Of().

-   Do ustawienia oczekiwanej wartości używamy operatora ==.

-   Zwracany typ to T.

-   Nie musimy więc odwoływać się do obiektu poprzez właściwość Object,
    gdyż jej już nie ma.

-   Aby później zmodyfikować taki mock (np. dodać Setup), należy
    posłużyć się metodą Mock.Get.

Ustawienie mocka w stylu funkcyjnym wygląda następująco:

ICustomer customerMock = Mock.Of(customer =\> customer.GetAge() == 16);

bool validate = validator.Validate(customerMock);

Dzięki syntaktyce funkcyjnej możemy w prosty sposób zbudować złożone
mocki. Np.:

ICustomer customerMock = Mock.Of(customer =\>

customer.FirstName == \"John\" &&

customer.LastName == \"Kowalski\" &&

customer.PercentageDiscount == 20 &&

customer.PhoneNumber == Mock.Of(number =\> number.MobileNumber ==
\"123-456-789\") &&

customer.Orders == new List

{

Mock.Of(order =\> order.Id == 23 && order.Price == 20.01m),

Mock.Of(order =\> order.Id == 65 && order.Price == 59.99m),

Mock.Of(order =\> order.Id == 82 && order.Price == 9.99m),

} &&

customer.GetAge() == 20);

Argument Matching 
-----------------

It.IsAny - gdy musimy przekazać zadany typ obiektu, który nie może być
null-em, ale jednocześnie nie przejmując się tym co jego właściwości lub
metody mogą zwrócić.

It.Is
-----

It.IsIn sprawdza czy porównywana wartość występuje na liście
zdefiniowanych wartości,

It.IsInRange sprawdza czy zadana wartość jest w podanym zakresie,

It.IsRegex
----------

Mockowanie metod z parametrem out
---------------------------------

<https://dariuszwozniak.net/posts/mockowanie-out>

Testowanie istniejącego już kodu
================================

Sposoby na wprowadzenie testów do już istniejącego kodu

1.  Przed dopisaniem nowego kodu napisz do niego testy.

2.  Przed naprawianiem błędu udowodnij testem, że błąd faktycznie
    występuje.

3.  Przed refactoringiem upewnij się, że niczego nie zepsujesz

Testy konwencji
---------------

-   wymuszają stosowanie coding guidelines

-   pilnują struktury projektu/klasy

-   weryfikują proces pisania kodu

Przykłady:

-   czy wszystkie klasy w danej dllce implementują interfejsy

-   czy nazwy interfejsów lub klas zaczynają się/kończą się jakąś
    wymaganą frazą (typu I w nazwie interfejsu)

-   testowanie zawartości poszczególnych plików -\> np. czy pliki
    zawierają jakieś/nie zawierają jakiś wyrażeń

-   czy są jakieś interfejsy bez implementacji

-   czy wszystko co jest potrzebne zarejestrowano w kontenerze IoC? (czy
    da się zresolvować wszystkie interesujące nas klasy)

Brak dedykowanych bibliotek, były próby:

<https://github.com/kkozmic/Norman>

<https://github.com/kkozmic/ConventionTests>

Inne
====

Jak testować statyczne metody/klasy?
------------------------------------

Jak testować metody prywatne?
-----------------------------

Czy powinniśmy testować wywołania Thread.Sleep, Task.Delay?
-----------------------------------------------------------

Jak testować metody, które korzystają z HttpClient?
---------------------------------------------------

Linki do materiałów

[https://github.com/unicodeveloper/awesome-tdd\#tdd-in-C\#](https://github.com/unicodeveloper/awesome-tdd#tdd-in-C)

<https://github.com/quozd/awesome-dotnet#testing>

<https://dariuszwozniak.net/posts/kurs-tdd-1-wstep/>

<https://dariuszwozniak.net/posts/mockowanie-out>

<https://www.youtube.com/watch?v=RPam7uk55fI>

<https://devstyle.pl/category/tech/tests/>

<https://devstyle.pl/2020/06/25/mega-pigula-wiedzy-o-testach-jednostkowych/>

<https://devstyle.pl/2013/04/22/testy-jednostkowe-materialy-do-nauki/>

<https://dzone.com/articles/5-easy-ways-to-write-hard-to-testcode/>

<https://www.toptal.com/qa/how-to-write-testable-code-and-why-it-matters/>
