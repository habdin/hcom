from datetime import date
from django.db import models
from django.contrib.auth.models import AbstractUser
from django.utils.translation import gettext_lazy as _


class City(models.Model):
    city = models.CharField(_("city"), blank=False, default="", max_length=100)

    def __str__(self):
        return self.city

    class Meta:
        verbose_name_plural = "Cities"


class User(AbstractUser):
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
    id = models.IntegerField(_("id"), primary_key=True, auto_created=True)
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


    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)


    def __str__(self, *args, **kwargs):
        return self.username
