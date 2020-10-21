# GCIS (Gestión Centralizada de Información de Servidores)

Este es un proyecto de una aplicación para mantener un inventario de los servidores, usuarios, software instalado, servicios web publicados. Consta de tres partes:

## Core.Utils

Es una librería para encriptar y desencriptar llaves que estamos guardando en el Registro de Windows.

## WCFGCIS

Es el servicio web WCF que sirve para realizar el CRUD de los datos.

## GCISApp

Es la aplicación WPF utilizada para ingresar la información de los servidores a la base de datos.
