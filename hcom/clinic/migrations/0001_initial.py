# Generated by Django 4.1.1 on 2023-03-12 06:09

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = []

    operations = [
        migrations.CreateModel(
            name="Appointment",
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
                (
                    "appointment_time",
                    models.TimeField(default=datetime.time, verbose_name="time"),
                ),
                (
                    "appointment_number",
                    models.SmallIntegerField(
                        blank=True, default="0", verbose_name="appointment number"
                    ),
                ),
            ],
        ),
        migrations.CreateModel(
            name="Clinic",
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
                ("opening_time", models.TimeField(verbose_name="Opening time")),
                ("closing_time", models.TimeField(verbose_name="Closing time")),
                ("is_archived", models.BooleanField(verbose_name="Inactive")),
            ],
        ),
        migrations.CreateModel(
            name="Patient",
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
                (
                    "first_name",
                    models.CharField(
                        default="", max_length=50, verbose_name="first name"
                    ),
                ),
                (
                    "last_name",
                    models.CharField(
                        default="", max_length=50, verbose_name="first name"
                    ),
                ),
                (
                    "id_number",
                    models.IntegerField(
                        blank=True,
                        null=True,
                        unique=True,
                        verbose_name="identity number",
                    ),
                ),
                (
                    "birth_date",
                    models.DateField(
                        default=datetime.date.today, verbose_name="date of birth"
                    ),
                ),
                (
                    "gender",
                    models.CharField(
                        choices=[("M", "Male"), ("F", "Female"), ("U", "Unspecified")],
                        default="",
                        max_length=1,
                        verbose_name="gender",
                    ),
                ),
                (
                    "marital_status",
                    models.CharField(
                        choices=[
                            ("S", "Single"),
                            ("M", "Married"),
                            ("D", "Divorced"),
                            ("W", "Widowed"),
                            ("U", "Unspecified"),
                        ],
                        default="",
                        max_length=1,
                        verbose_name="marital status",
                    ),
                ),
                (
                    "occupation",
                    models.CharField(
                        blank=True,
                        default="",
                        max_length=120,
                        null=True,
                        verbose_name="occupation",
                    ),
                ),
                (
                    "image",
                    models.ImageField(
                        default="default.jpg",
                        max_length=120,
                        upload_to="patients",
                        verbose_name="image",
                    ),
                ),
            ],
            options={
                "verbose_name_plural": "Patients",
            },
        ),
    ]
