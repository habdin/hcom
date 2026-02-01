from django.contrib import admin

from .models import Appointment, Clinic, Patient, Physician, Speciality

# Register your models here.


class PatientAdmin(admin.ModelAdmin):
    list_display = [
        "first_name",
        "last_name",
        "id_number",
        "gender",
        "marital_status",
        "occupation",
    ]
    search_fields = [
        "username",
        "first_name",
        "last_name",
    ]
    fields = [
        "first_name",
        "last_name",
        "id_number",
        "gender",
        "marital_status",
        "city",
        "occupation",
    ]
    list_per_page = 8


class PhysicianAdmin(admin.ModelAdmin):
    list_display = [
        "id",
        "first_name",
        "last_name",
        "id_number",
        "gender",
        "marital_status",
        "occupation",
    ]
    search_fields = [
        "username",
        "first_name",
        "last_name",
        "email",
    ]
    fields = [
        "username",
        "first_name",
        "last_name",
        "email",
        "id_number",
        "gender",
        "marital_status",
        "image",
        "speciality",
    ]
    list_per_page = 8


admin.site.register(Patient, PatientAdmin)
admin.site.register(Physician, PhysicianAdmin)
admin.site.register(Clinic)
admin.site.register(Speciality)
admin.site.register(Appointment)
