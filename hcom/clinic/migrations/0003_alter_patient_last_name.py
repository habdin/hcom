# Generated by Django 4.1.1 on 2023-03-12 15:56

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ("clinic", "0002_initial"),
    ]

    operations = [
        migrations.AlterField(
            model_name="patient",
            name="last_name",
            field=models.CharField(default="", max_length=50, verbose_name="last name"),
        ),
    ]
