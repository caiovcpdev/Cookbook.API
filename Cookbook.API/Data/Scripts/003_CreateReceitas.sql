CREATE TABLE Receitas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    CategoriaId INT NOT NULL,
    Nome NVARCHAR(150) NOT NULL,
    Descricao NVARCHAR(1000) NULL,
    TempoPreparo INT NOT NULL,
    Porcoes INT NOT NULL,
    Dificuldade NVARCHAR(20) NOT NULL,
    Imagem NVARCHAR(500) NULL,
    DataCadastro DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Receitas_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Receitas_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);