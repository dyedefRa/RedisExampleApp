REDIS-GERCEK ORNEK PROJESI

In-Memory db kullanacağız.
Test anında veya hızlı proje test etmek gerektiğinde bu db kullanılır.

NUGET>

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.InMemory

------------------------
Redis String >

Redis List > 
Datalar bir alta eklnir.

Redis Set > 
tekrar eden data yok
Datalar random olarak kaydedilir.

Redis SortedSet >
Sıralamaya göre araya data ekleyebiliyoruz.
Score ile.

Redis Hash >
Key value şeklinde tutulur.
Id => value product class


--------------------------------

Docker container oluştur ve çalıştır.

PowerShell >
docker run -d -p 6379:6379 --name redis redis


docker ps -a   > tüm containleri getir

docker start {containerid}
docker start 274 >  containeri run et.
docker exec -it 274 ps > içeriye gir.
# redis-cli >
ping

/////////////////////////////////////////////////////





