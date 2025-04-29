# ECondo [![.NET](https://github.com/infirit89/Condominium-Management-App/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/infirit89/Condominium-Management-App/actions/workflows/dotnet.yml)

![Econdo Home Page](/econdoHome.png)

ECondo is condominium managment system designed to offer to easiest possible experience

---
## :star2: Features

1. Entrance Manager
   - Can create/delete/update properties for the respective entrance.
   - Can create/delete/update occupants (owners, tenants and users) for the properties
   - Can create bills and monitor payments using [Stripe](https://stripe.com)
2. Owners
    - Can add/delete/update the tenants and users of their properties
    - Can pay bills for their properties
3. Tenants and Users
    - Can pay bills for the properties they occupy

---
## :hammer_and_wrench: How to build

> [!NOTE]
>
> ECondo's API and Client both support **HTTP** and **HTTPS**.
>
> However, for development purposes, it is recommended to use **HTTP** as it is easier to set up and requires less configuration.
>
> - In **development mode**, both the API and the Client default to **HTTP**.
> - In **production (release) mode**, they default to **HTTPS** for secure communication.

### Prerequisites

Before you build the project, make sure the following tools are installed on your system:

- [Docker](https://www.docker.com/)  
  Required for containerizing and running the backend services.

- [Node.js](https://nodejs.org/) (includes `npm`)  
  Required for building and running the frontend client.

---
Now to get started, clone the repository to your local machine using `git`:

```bash
git clone https://github.com/infirit89/Condominium-Management-App
```

Before building the project, make sure to create an `appsettings.json` file in the ECondo.Api directory.  
Use the provided `example.appsettings.json` as a reference, and fill in the appropriate credentials for your environment.

---

Once that's done, you can build and start the API using Docker Compose:

```bash
cd Condominium-Management-App

cd backend

docker compose up --build
```

> [!NOTE]
>
> :white_check_mark: Successful Startup
>
> You'll know the app has started successfully when you see the following line in the logs:
> ```bash
> Content root path: /app
> ```


After that you have to setup the client

```bash
cd ..

cd frontend/econdo.client

npm i
```

Before building the client, make sure to create a .env file.
Use the provided `.env.example` as a reference, and fill in the appropriate credentials for your environment.

---

Once that's done, you can build and start the client:

```bash
npm run dev
```

> [!IMPORTANT]
>
> Starting the client using this method will launch the app in **development mode**.
>
> In this mode, **Next.js compiles pages on-demand** — meaning the first time a user visits a page, there may be a short delay while Next.js builds it. This is completely normal.
>
> :clock3: Please be patient during these initial loads.
>
> In **production (release) mode**, all pages are **precompiled ahead of time**, so this delay does not occur.

### :tada: That's All!

Congratulations! :confetti_ball: You've successfully completed the build and setup process for **ECondo**.

You can now enjoy exploring and using the platform.  
Whether you're managing properties or just trying things out — you're all set to go!


