# SoundVibe - Cloud Music Streaming Service

A full-stack web application for uploading, storing, and streaming music. Built with a clean architecture backend and a modern React frontend.

## 🎵 Features

*   **Streaming:** High-quality audio streaming from Yandex Cloud S3.
*   **Personal Library:** Upload, manage, and organize your personal music collection.
*   **Discovery:** Naive Bayesian recommender system suggests new music based on your listening habits.
*   **Modern UI:** Responsive and intuitive React-based user interface.

## 🛠️ Tech Stack

*   **Frontend:** React, JavaScript
*   **Backend:** ASP.NET Core Web API, Clean Architecture
*   **Data Access:** Entity Framework Core, PostgreSQL
*   **Cloud Storage:** Yandex Cloud S3 (Object Storage)
*   **Authentication:** JWT Bearer Tokens

## 🏗️ Architecture

The solution follows Clean Architecture principles, ensuring separation of concerns and testability:
*   `Core` (Entities)
*   `Application` (Interfaces, Services)
*   `Infrastructure` (Data, Cloud Storage, Email)
*   `WebAPI` (Controllers)

<img width="710" height="688" alt="Снимок экрана 2024-07-09 220142" src="https://github.com/user-attachments/assets/d30b492c-fb78-44c0-9dd0-3d5a22022ae1" />
