#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.db import models
from users.models import User, City 
from datetime import date, time
from django.utils.translation import gettext_lazy as _


"""
Each Clinic will have the following members: Patient, Physician, Nurse, Secretary, Dustmen
Besides the personel, any clinic will also have: work days, vacation day, work times, address information.

Each Patient has complaint, symptoms, signs, investigations, medical treatment, intervention, surgical treatment

Investigations can be laboratory, radiology, endoscopic, others.
"""


class Patient(models.Model):
    GENDER_CHOICES = [
        ('M', 'Male'),
        ('F', 'Female'),
        ('U', 'Unspecified')
    ]
    MARITAL_STATUS_CHOICES = [
        ('S', 'Single'),
        ('M', 'Married'),
        ('D', 'Divorced'),
        ('W', 'Widowed'),
        ('U', 'Unspecified')
    ]
    first_name = models.CharField(
        _("first name"), max_length=50, blank=False, null=False, default="")
    last_name = models.CharField(
        _("last name"), max_length=50, blank=False, null=False, default="")
    id_number = models.BigIntegerField(
        _("identity number"), unique=True, blank=True, null=True)
    birth_date = models.DateField(
        _("date of birth"), blank=False, default=date.today)
    gender = models.CharField(
        _('gender'), default="", max_length=1, choices=GENDER_CHOICES)
    marital_status = models.CharField(
        _("marital status"), default="", max_length=1,
        choices=MARITAL_STATUS_CHOICES)
    occupation = models.CharField(
        _("occupation"), max_length=120, null=True, blank=True, default=""
    )
    city = models.ForeignKey(City, on_delete=models.CASCADE, default="")
    image = models.ImageField(
        _("image"),upload_to="patients", max_length=120, default="default.jpg")


    def __str__(self):
        return f"{self.first_name} {self.last_name}"

    class Meta:
        verbose_name_plural = "Patients"
        

class Speciality(models.Model):
    name = models.CharField(
        _("name"), max_length=100, blank=False, null=False, default="")

    class Meta:
        verbose_name_plural = "Specialities"

    def __str__(self):
        return self.name


class Physician(User):
    speciality = models.ForeignKey(
        Speciality, on_delete=models.CASCADE, max_length=100, default=""
    )
    image = models.ImageField(
        _("image"), upload_to="physicians", max_length=120, default="default.jpg")

    def __str__(self):
        return f"{self.first_name} {self.last_name}"

    class Meta:
        verbose_name_plural = "Physicians"


class Clinic(models.Model):
    opening_time = models.TimeField("Opening time", blank=False)
    closing_time = models.TimeField("Closing time", blank=False)
    physician = models.ForeignKey(
        Physician, on_delete=models.CASCADE, max_length=50, default=""
    )
    is_archived = models.BooleanField("Inactive", null=False)

    def __str__(self):
        return f"Dr. {self.physician} 's clinic"


class Appointment(models.Model):
    appointment_time = models.TimeField("time", blank=False, default=time)
    clinic = models.ForeignKey(
        Clinic, on_delete=models.CASCADE, max_length=100, default=""
    )
    patient = models.ForeignKey(
        Patient, on_delete=models.CASCADE, max_length=100, default=""
    )
    appointment_number = models.SmallIntegerField(
        "appointment number", blank=True, null=False, default="0")

    def __str__(self):
        return f"Time is {self.appointment_time}"
