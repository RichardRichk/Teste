CREATE DATABASE todolist
USE todolist

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Prioridade (
    IdPrioridade INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(50) NOT NULL
);

CREATE TABLE [Status] (
    IdStatus INT PRIMARY KEY IDENTITY(1,1),
    [Status] NVARCHAR(50) NOT NULL
);

CREATE TABLE Tarefa (
    IdTarefa INT PRIMARY KEY IDENTITY(1,1),
    Descricao NVARCHAR(MAX),
		Setor NVARCHAR(50),
		DataCadastro DATE,
    IdUsuario INT,
    IdPrioridade INT,
    IdStatus INT,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdPrioridade) REFERENCES Prioridade(IdPrioridade),
    FOREIGN KEY (IdStatus) REFERENCES [Status](IdStatus)
);

-- Inserir dados iniciais para Prioridades e Status
INSERT INTO Prioridade (Titulo) VALUES ('Baixa'), ('Média'), ('Alta');
INSERT INTO [Status] ([Status]) VALUES ('A fazer'), ('Fazendo'), ('Pronto');
