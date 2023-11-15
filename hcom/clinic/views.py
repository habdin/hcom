# vim: foldmethod=indent
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.shortcuts import render
from django.views.generic import DetailView, ListView

from .models import Appointment, Clinic, Patient, Physician, Speciality

# def index(request):
#    """
#    View for clinic manager home page.
#    """
#    clinics = Clinic.objects.all()
#    #patients = [
#    #    {
#    #        'Name': 'Richard Johnson',
#    #        'Time': '6:30 PM',
#    #    },
#    #    {
#    #        'Name': 'Mary Antoinette',
#    #        "Time": '7:00 PM',
#    #    }
#    #]
#    context = {
#        'title': 'Clinic Manager',
#        'clinics': clinics,
#        #'patients': patients,
#        #'doctor_name': "Hassan Abdin"
#    }
#    return render(request, 'clinic/index.html', context)


class ClinicListView(ListView):
    template_name = "clinic/index.html"
    context_object_name = "clinics"

    def get_queryset(self):
        return Clinic.objects.all()

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["title"] = "Clinics list"
        return context


def Clinic_Detail(request, id):
    """The clinic should show list of appointments if any"""
    clinic = Clinic.objects.get(id=id)
    appointment_list = clinic.appointment_set.all()
    patient_list = Patient.objects.all()
    context = {
        "clinic": clinic,
        "appointment_list": appointment_list,
        "patient_list": patient_list,
        "title": "Clinic Patients and appointments",
    }
    return render(request, "clinic/detail.html", context=context)


class ClinicDetailView(DetailView):
    model = Clinic
    template_name = "clinic/detail.html"

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["appointment_list"] = Appointment.objects.all()
        context["patient_list"] = Patient.objects.all()
        return context
