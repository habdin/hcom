# Generated by Django 4.1.1 on 2023-04-15 00:11

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = []

    operations = [
        migrations.CreateModel(
            name="Dummy",
            fields=[
                (
                    "id",
                    models.BigAutoField(
                        auto_created=True,
                        primary_key=True,
                        serialize=False,
                        verbose_name="ID",
                    ),
                ),
                ("name", models.CharField(max_length=60, verbose_name="name")),
                ("category", models.CharField(max_length=60, verbose_name="category")),
            ],
            options={
                "verbose_name_plural": "Dummies",
            },
        ),
    ]
