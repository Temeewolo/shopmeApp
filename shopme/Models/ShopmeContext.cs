using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace shopme.Models;

public partial class ShopmeContext : DbContext
{
    public ShopmeContext(DbContextOptions<ShopmeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Comporte> Comportes { get; set; }

    public virtual DbSet<Lignecommande> Lignecommandes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<Recevoir> Recevoirs { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.IdCommande).HasName("PRIMARY");

            entity.ToTable("commande");

            entity.HasIndex(e => e.IdUtilisareur, "idUtilisareur");

            entity.Property(e => e.IdCommande)
                .HasColumnType("int(11)")
                .HasColumnName("idCommande");
            entity.Property(e => e.CodeCommande)
                .HasMaxLength(6)
                .HasColumnName("codeCommande");
            entity.Property(e => e.DateCommande)
                .HasColumnType("datetime")
                .HasColumnName("dateCommande");
            entity.Property(e => e.IdUtilisareur)
                .HasColumnType("int(11)")
                .HasColumnName("idUtilisareur");
            entity.Property(e => e.StatutCommande).HasColumnName("statutCommande");

            entity.HasOne(d => d.IdUtilisareurNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.IdUtilisareur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("commande_ibfk_1");
        });

        modelBuilder.Entity<Comporte>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("comporte");

            entity.HasIndex(e => e.IdCommande, "idCommande");

            entity.HasIndex(e => e.IdLigneCommande, "idLigneCommande");

            entity.Property(e => e.IdCommande)
                .HasColumnType("int(11)")
                .HasColumnName("idCommande");
            entity.Property(e => e.IdLigneCommande)
                .HasColumnType("int(11)")
                .HasColumnName("idLigneCommande");
            entity.Property(e => e.PrixLigneCommande)
                .HasColumnType("int(11)")
                .HasColumnName("prixLigneCommande");
            entity.Property(e => e.QuantiteLigneCommande)
                .HasColumnType("int(11)")
                .HasColumnName("quantiteLigneCommande");

            entity.HasOne(d => d.IdCommandeNavigation).WithMany()
                .HasForeignKey(d => d.IdCommande)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comporte_ibfk_1");

            entity.HasOne(d => d.IdLigneCommandeNavigation).WithMany()
                .HasForeignKey(d => d.IdLigneCommande)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comporte_ibfk_2");
        });

        modelBuilder.Entity<Lignecommande>(entity =>
        {
            entity.HasKey(e => e.IdLigneCommande).HasName("PRIMARY");

            entity.ToTable("lignecommande");

            entity.HasIndex(e => e.IdProduit, "idProduit");

            entity.Property(e => e.IdLigneCommande)
                .HasColumnType("int(11)")
                .HasColumnName("idLigneCommande");
            entity.Property(e => e.IdProduit)
                .HasColumnType("int(11)")
                .HasColumnName("idProduit");
            entity.Property(e => e.ProduitLigneCommande).HasMaxLength(255);
            entity.Property(e => e.StatutLigneCommande).HasColumnName("statutLigneCommande");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.Lignecommandes)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lignecommande_ibfk_1");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.IdNotification).HasName("PRIMARY");

            entity.ToTable("notification");

            entity.Property(e => e.IdNotification)
                .HasColumnType("int(11)")
                .HasColumnName("idNotification");
            entity.Property(e => e.DateNptification)
                .HasColumnType("datetime")
                .HasColumnName("dateNptification");
            entity.Property(e => e.MessageNotification)
                .HasMaxLength(255)
                .HasColumnName("messageNotification");
            entity.Property(e => e.StatutNotification).HasColumnName("statutNotification");
            entity.Property(e => e.TypeNotification)
                .HasMaxLength(255)
                .HasColumnName("typeNotification");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.IdProduit).HasName("PRIMARY");

            entity.ToTable("produit");

            entity.Property(e => e.IdProduit)
                .HasColumnType("int(11)")
                .HasColumnName("idProduit");
            entity.Property(e => e.CodeProduit)
                .HasMaxLength(6)
                .HasColumnName("codeProduit");
            entity.Property(e => e.DateFabricationProduit).HasColumnName("dateFabricationProduit");
            entity.Property(e => e.DatePeremptionProduit).HasColumnName("datePeremptionProduit");
            entity.Property(e => e.NomProduit)
                .HasMaxLength(255)
                .HasColumnName("nomProduit");
            entity.Property(e => e.PrixProduit)
                .HasColumnType("int(11)")
                .HasColumnName("prixProduit");
            entity.Property(e => e.TypeProduit)
                .HasMaxLength(40)
                .HasColumnName("typeProduit");
            entity.Property(e => e.VisibiliteProduit).HasColumnName("visibiliteProduit");
        });

        modelBuilder.Entity<Recevoir>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("recevoir");

            entity.HasIndex(e => e.IdNotification, "idNotification");

            entity.HasIndex(e => e.IdUtilisareur, "idUtilisareur");

            entity.Property(e => e.IdNotification)
                .HasColumnType("int(11)")
                .HasColumnName("idNotification");
            entity.Property(e => e.IdUtilisareur)
                .HasColumnType("int(11)")
                .HasColumnName("idUtilisareur");

            entity.HasOne(d => d.IdNotificationNavigation).WithMany()
                .HasForeignKey(d => d.IdNotification)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recevoir_ibfk_1");

            entity.HasOne(d => d.IdUtilisareurNavigation).WithMany()
                .HasForeignKey(d => d.IdUtilisareur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recevoir_ibfk_2");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUtilisareur).HasName("PRIMARY");

            entity.ToTable("utilisateur");

            entity.Property(e => e.IdUtilisareur)
                .HasColumnType("int(11)")
                .HasColumnName("idUtilisareur");
            entity.Property(e => e.FonctionUtilisateur)
                .HasMaxLength(40)
                .HasColumnName("fonctionUtilisateur");
            entity.Property(e => e.InscriptionUtilisateur)
                .HasColumnType("datetime")
                .HasColumnName("inscriptionUtilisateur");
            entity.Property(e => e.MotDePasseUtilisateur)
                .HasMaxLength(255)
                .HasColumnName("motDePasseUtilisateur");
            entity.Property(e => e.NomUtilisateur)
                .HasMaxLength(40)
                .HasColumnName("nomUtilisateur");
            entity.Property(e => e.NumeroUtilisateur)
                .HasColumnType("int(9)")
                .HasColumnName("numeroUtilisateur");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
