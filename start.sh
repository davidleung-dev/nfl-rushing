cd nfl-rushing
docker build -t nfl-rush-app-img .

cd ../NflRushingApi
docker build -t nfl-rush-api-img .

cd ../

docker run -d -p 5001:80 --name nfl-rush-api nfl-rush-api-img
docker run -d -p 8080:80 --name nfl-rush-app nfl-rush-app-img

